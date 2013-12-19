﻿using System.IO;
using System.IO.Compression;
using System.Text;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Utils.ContentEncoding
{
    [Subject(typeof(GZipContentEncoder))]
    class when_gzip_encoder_is_used
    {
        static GZipContentEncoder encoder;
        static MemoryStream comressed_stream;

        Establish context = 
            () => encoder = new GZipContentEncoder();

        Because of = () =>
        {
            using (var comressedStream = new MemoryStream())
            {
                using (var uncomressedStream = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")))
                using (var compressionStream = encoder.GetCompressionStream(comressedStream))
                {
                    uncomressedStream.CopyTo(compressionStream);
                }

                comressed_stream = new MemoryStream(comressedStream.ToArray());
            }
        };

        It should_be_able_to_comress =
            () => DecomressData().ShouldEqual("Hello World!");

        static string DecomressData()
        {
            using (var data = new MemoryStream())
            {
                using (var decompressionStream = new GZipStream(comressed_stream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(data);
                }

                return Encoding.UTF8.GetString(data.ToArray());
            }
        }
    }
}