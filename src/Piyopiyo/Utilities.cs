using System;
using System.IO;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenMLTD.Piyopiyo {
    internal static class Utilities {

        public const int DefaultBufferSize = 10240;

        [NotNull]
        public static readonly Encoding Utf8;

        [NotNull]
        private static readonly ThreadLocal<JsonSerializer> LocalSerializer;

        static Utilities() {
            LocalSerializer = new ThreadLocal<JsonSerializer>(JsonSerializer.CreateDefault);
            Utf8 = new UTF8Encoding(false);
        }

        [NotNull]
        public static JsonSerializer Serializer => LocalSerializer.Value;

        [ContractAnnotation("null => true; notnull => false")]
        public static bool IsNull([CanBeNull] this JToken obj) {
            return obj == null || obj.Type == JTokenType.Null;
        }

        [CanBeNull]
        public static T Deserialize<T>([NotNull] this JsonSerializer serializer, [NotNull] Stream jsonStream) {
            using (var textReader = new StreamReader(jsonStream, Utf8, true, DefaultBufferSize, true)) {
                using (var jsonReader = new JsonTextReader(textReader)) {
                    return serializer.Deserialize<T>(jsonReader);
                }
            }
        }

        [CanBeNull]
        public static T Deserialize<T>([NotNull] this JsonSerializer serializer, [NotNull] string json) {
            var bytes = Utf8.GetBytes(json);

            using (var memoryStream = new MemoryStream(bytes, false)) {
                using (var textReader = new StreamReader(memoryStream, Utf8)) {
                    using (var jsonReader = new JsonTextReader(textReader)) {
                        return serializer.Deserialize<T>(jsonReader);
                    }
                }
            }
        }

        [CanBeNull]
        public static T Deserialize<T>([NotNull] this JsonSerializer serializer, [NotNull] byte[] json) {
            using (var memoryStream = new MemoryStream(json, false)) {
                using (var textReader = new StreamReader(memoryStream, Utf8)) {
                    using (var jsonReader = new JsonTextReader(textReader)) {
                        return serializer.Deserialize<T>(jsonReader);
                    }
                }
            }
        }

        [NotNull]
        public static string StreamToString([NotNull] this Stream stream, [NotNull] Encoding encoding) {
            byte[] bytes;

            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return encoding.GetString(bytes);
        }

        [NotNull]
        public static byte[] ReadAllBytes([NotNull] this Stream stream) {
            byte[] result;

            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);

                result = memoryStream.ToArray();
            }

            return result;
        }

        [NotNull]
        public static Stream CreateByteStream([NotNull] byte[] bytes) {
            // MemoryStream's Dispose() does not actually do anything except for suppressing Finalize()
            return new MemoryStream(bytes, false);
        }

        [CanBeNull]
        private static object GetDefaultValueOf([NotNull] Type t) {
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }

    }
}
