﻿using System.IO;

namespace Powel.BotClient.Infrastructure.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            byte[] bytes;

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                bytes = ms.ToArray();
            }

            return bytes;
        }
    }
}