using System;
using System.Collections.Generic;
using System.Text;

namespace DocaLabs.Http.Client.Utils
{
    public class HeaderParser
    {
        const int Ascii7BitMaxValue = 127;

        // characters allowed in tokens
        static readonly bool[] Ttext = new bool[128];

        // characters allowed in quoted strings (not including Unicode)
        static readonly bool[] Qtext = new bool[128];

        readonly string _headerValue;
        int _offset;

        static HeaderParser()
        {
            InitializeTtext();

            InitializeQtext();
        }

        static void InitializeQtext()
        {
            // qtext = %d1-9 / %d11 / %d12 / %d14-33 / %d35-91 / %d93-127
            for (var i = 1; i <= 9; i++)
                Qtext[i] = true;

            Qtext[11] = true;
            Qtext[12] = true;

            for (var i = 14; i <= 33; i++)
                Qtext[i] = true;

            for (var i = 35; i <= 91; i++)
                Qtext[i] = true;

            for (var i = 93; i <= 127; i++)
                Qtext[i] = true;
        }

        static void InitializeTtext()
        {
            // ttext = %d33-126 except '()<>@,;:\"/[]?='
            for (var i = 33; i <= 126; i++)
                Ttext[i] = true;

            Ttext['('] = false;
            Ttext[')'] = false;
            Ttext['<'] = false;
            Ttext['>'] = false;
            Ttext['@'] = false;
            Ttext[','] = false;
            Ttext[';'] = false;
            Ttext[':'] = false;
            Ttext['\\'] = false;
            Ttext['"'] = false;
            Ttext['/'] = false;
            Ttext['['] = false;
            Ttext[']'] = false;
            Ttext['?'] = false;
            Ttext['='] = false;
        }

        public HeaderParser(string value)
        {
            _headerValue = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
            _offset = 0;
        }

        public bool IsEOF()
        {
            return _offset >= _headerValue.Length;
        }

        public bool Is(char ch)
        {
            return !IsEOF() && _headerValue[_offset++] == ch;
        }

        public bool SkipCfws()
        {
            var comments = 0;

            for (; _offset < _headerValue.Length; _offset++)
            {
                if (_headerValue[_offset] > 127)
                    throw new FormatException(string.Format(Resources.Text.invalid_header_character, _headerValue[_offset], _headerValue));

                if (_headerValue[_offset] == '\\' && comments > 0)
                    _offset += 2;
                else if (_headerValue[_offset] == '(')
                    comments++;
                else if (_headerValue[_offset] == ')')
                    comments--;
                else if (_headerValue[_offset] != ' ' && _headerValue[_offset] != '\t' && comments == 0)
                    return true;

                if (comments < 0)
                    throw new FormatException(string.Format(Resources.Text.invalid_header_character, _headerValue[_offset], _headerValue));
            }

            //returns false if end of string 
            return false;
        }

        public string ReadToken()
        {
            var start = _offset;
            for (; _offset < _headerValue.Length; _offset++)
            {
                if (_headerValue[_offset] > Ascii7BitMaxValue)
                    throw new FormatException(string.Format(Resources.Text.invalid_header_character, _headerValue[_offset], _headerValue));

                if (!Ttext[_headerValue[_offset]])
                    break;
            }

            if (start == _offset)
                throw new FormatException(string.Format(Resources.Text.invalid_header_character, _headerValue[_offset], _headerValue));

            return _headerValue.Substring(start, _offset - start);
        }

        public string ReadQuotedString()
        {
            // assume first char is the opening quote
            ++_offset;

            var start = _offset;
            var builder = new StringBuilder();

            for (; _offset < _headerValue.Length; _offset++)
            {
                if (_headerValue[_offset] == '\\')
                {
                    builder.Append(_headerValue, start, _offset - start);
                    start = ++_offset;
                }
                else if (_headerValue[_offset] == '"')
                {
                    builder.Append(_headerValue, start, _offset - start);
                    _offset++;
                    return builder.ToString();
                }
                else if (_headerValue[_offset] == '=' && _headerValue.Length > _offset + 3 && _headerValue[_offset + 1] == '\r' && _headerValue[_offset + 2] == '\n' &&
                         (_headerValue[_offset + 3] == ' ' || _headerValue[_offset + 3] == '\t'))
                {
                    //it's a soft CRLF so it's OK
                    _offset += 3;
                }
                // not permitting Unicode, in which case Unicode is a formatting error
                else if (_headerValue[_offset] > Ascii7BitMaxValue || !Qtext[_headerValue[_offset]])
                {
                    throw new FormatException(string.Format(Resources.Text.invalid_header_character, _headerValue[_offset], _headerValue));
                }
            }

            throw new FormatException(string.Format(Resources.Text.malformed_header, _headerValue));
        }

        public Dictionary<string, string> ReadParameters()
        {
            var parameters = new Dictionary<string, string>();

            while (SkipCfws())
            {
                if (!Is(';'))
                    throw new FormatException(string.Format(Resources.Text.malformed_header, _headerValue));

                if (!SkipCfws())
                    break;

                var paramAttribute = ReadParameterAttribute();

                if (string.IsNullOrWhiteSpace(paramAttribute))
                    throw new FormatException(string.Format(Resources.Text.malformed_header, _headerValue));

                if (!Is('='))
                    throw new FormatException(string.Format(Resources.Text.malformed_header, _headerValue));

                if (!SkipCfws())
                    throw new FormatException(string.Format(Resources.Text.malformed_header, _headerValue));

                var paramValue = ReadParameterValue();

                if (paramValue == null)
                    throw new FormatException(string.Format(Resources.Text.malformed_header, _headerValue));

                parameters.Add(paramAttribute, paramValue);
            }

            return parameters;
        }

        public string ReadParameterAttribute()
        {
            return !SkipCfws() 
                ? null 
                : ReadToken();
        }

        public string ReadParameterValue()
        {
            return _headerValue[_offset] == '"'
                ? ReadQuotedString()
                : ReadToken();
        }
    }
}