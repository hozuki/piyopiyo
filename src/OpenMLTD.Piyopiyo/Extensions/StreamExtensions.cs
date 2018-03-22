using System;
using System.IO;
using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Net;

namespace OpenMLTD.Piyopiyo.Extensions {
    public static class StreamExtensions {

        [NotNull]
        public static byte[] ReadMaxConstrained([NotNull] this Stream stream, long maxBytesToRead) {
            if (maxBytesToRead <= 0) {
                return JsonRpcServerHelper.EmptyBytes;
            }

            const int bufferSize = 10240;

            var buffer = new byte[bufferSize];

            var maxLeft = maxBytesToRead;
            var toRead = (int)Math.Min(bufferSize, maxLeft);

            byte[] result;

            using (var memoryStream = new MemoryStream()) {
                while (true) {
                    var read = stream.Read(buffer, 0, toRead);

                    if (read > 0) {
                        memoryStream.Write(buffer, 0, read);

                        maxLeft -= read;
                        toRead = (int)Math.Min(bufferSize, maxLeft);
                    }

                    if (read <= 0 || maxLeft <= 0) {
                        break;
                    }
                }

                result = memoryStream.ToArray();
            }

            return result;
        }

    }
}
