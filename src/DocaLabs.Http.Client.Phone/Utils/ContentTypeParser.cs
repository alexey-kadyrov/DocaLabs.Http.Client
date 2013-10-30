using System;
using System.Collections.Generic;
using System.Text;

namespace DocaLabs.Http.Client.Utils
{
    public class ContentTypeParser
    {
        const int Ascii7BitMaxValue = 127;

        // characters allowed in tokens
        static readonly bool[] Ttext = new bool[128];

        // characters allowed in quoted strings (not including Unicode)
        static readonly bool[] Qtext = new bool[128];

        readonly string _contentType;
        int _offset;

        static ContentTypeParser()
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


            // fqtext = %d1-9 / %d11 / %d12 / %d14-33 / %d35-91 / %d93-127
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

        public ContentTypeParser(string contentType)
        {
            _contentType = string.IsNullOrWhiteSpace(contentType) ? ContentType.Default : contentType;
            _offset = 0;
        }

        public ContentType Parse()
        {
            var parameters = new Dictionary<string, string>();

            var mediaType = ReadToken();
            if (string.IsNullOrWhiteSpace(mediaType) || _offset >= _contentType.Length || _contentType[_offset++] != '/')
                throw new FormatException("Invalid content type.");

            var subType = ReadToken();
            if (string.IsNullOrWhiteSpace(subType))
                throw new FormatException("Invalid content type.");

            while (SkipCfws())
            {
                if (_contentType[_offset++] != ';')
                    throw new FormatException("Invalid content type.");

                if (!SkipCfws())
                    break;

                var paramAttribute = ReadParameterAttribute();

                if (string.IsNullOrWhiteSpace(paramAttribute))
                    throw new FormatException("Invalid content type.");

                if (_offset >= _contentType.Length || _contentType[_offset++] != '=')
                    throw new FormatException("Invalid content type.");

                if (!SkipCfws())
                    throw new FormatException("Invalid content type.");

                var paramValue = _contentType[_offset] == '"'
                    ? ReadQuotedString()
                    : ReadToken();

                if (paramValue == null)
                    throw new FormatException("Invalid content type.");

                parameters.Add(paramAttribute, paramValue);
            }

            return new ContentType(_contentType, mediaType, subType, parameters);
        }

        string ReadToken()
        {
            var start = _offset;
            for (; _offset < _contentType.Length; _offset++)
            {
                if (_contentType[_offset] > Ascii7BitMaxValue)
                    throw new FormatException(string.Format("Invalid content type - invalid character {0}.", _contentType[_offset]));
                
                if (!Ttext[_contentType[_offset]])
                    break;
            }

            if (start == _offset)
                throw new FormatException(string.Format("Invalid content type - invalid character {0}.", _contentType[_offset]));

            return _contentType.Substring(start, _offset - start);
        }

        bool SkipCfws()
        {
            var comments = 0;
            for (; _offset < _contentType.Length; _offset++)
            {
                if (_contentType[_offset] > 127)
                    throw new FormatException(string.Format("Invalid content type - invalid character {0}.", _contentType[_offset]));
                
                if (_contentType[_offset] == '\\' && comments > 0)
                    _offset += 2;
                else if (_contentType[_offset] == '(')
                    comments++;
                else if (_contentType[_offset] == ')')
                    comments--;
                else if (_contentType[_offset] != ' ' && _contentType[_offset] != '\t' && comments == 0)
                    return true;

                if (comments < 0)
                    throw new FormatException(string.Format("Invalid content type - invalid character {0}.", _contentType[_offset]));
            }

            //returns false if end of string 
            return false;
        }

        string ReadParameterAttribute()
        {
            return !SkipCfws() 
                ? null 
                : ReadToken();
        }

        string ReadQuotedString()
        {
            // assume first char is the opening quote
            ++_offset;

            var start = _offset;
            var builder = new StringBuilder();

            for (; _offset < _contentType.Length; _offset++)
            {
                if (_contentType[_offset] == '\\')
                {
                    builder.Append(_contentType, start, _offset - start);
                    start = ++_offset;
                }
                else if (_contentType[_offset] == '"')
                {
                    builder.Append(_contentType, start, _offset - start);
                    _offset++;
                    return builder.ToString();
                }
                else if (_contentType[_offset] == '=' && _contentType.Length > _offset + 3 && _contentType[_offset + 1] == '\r' && _contentType[_offset + 2] == '\n' &&
                         (_contentType[_offset + 3] == ' ' || _contentType[_offset + 3] == '\t'))
                {
                    //it's a soft crlf so it's ok
                    _offset += 3;
                }
                    //not permitting unicode, in which case unicode is a formatting error
                else if (_contentType[_offset] > Ascii7BitMaxValue || !Qtext[_contentType[_offset]])
                {
                    throw new FormatException(string.Format("Invalid content type - invalid character {0}.", _contentType[_offset]));
                }
            }

            throw new FormatException("Malformed Header.");
        }
    }
}