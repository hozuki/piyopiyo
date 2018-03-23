using System;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenMLTD.Piyopiyo {
    public static class BvspHelper {

        public const string BvspContentType = "application/jsonrpc-bvsp";
        public const string BvspCharSet = "utf-8";

        [NotNull]
        public static readonly byte[] EmptyBytes = new byte[0];

        [NotNull]
        public static readonly Encoding Utf8WithoutBom = new UTF8Encoding(false);

        [NotNull]
        public static byte[] JsonSerializeToByteArray([NotNull] object obj) {
            byte[] result;

            using (var memoryStream = new MemoryStream()) {
                using (var textWriter = new StreamWriter(memoryStream, Utf8WithoutBom)) {
                    JsonSerializer.Value.Serialize(textWriter, obj);
                }

                result = memoryStream.ToArray();
            }

            return result;
        }

        [NotNull]
        public static string JsonSerializeToString([NotNull] object obj) {
            var bytes = JsonSerializeToByteArray(obj);
            return Utf8WithoutBom.GetString(bytes);
        }

        [CanBeNull]
        public static T JsonDeserialize<T>([NotNull] string str) {
            var bytes = Utf8WithoutBom.GetBytes(str);
            return JsonDeserialize<T>(bytes);
        }

        [CanBeNull]
        public static T JsonDeserialize<T>([NotNull] byte[] data) {
            var t = typeof(T);

            if (t.IsArray) {
                throw new NotSupportedException("This method does not support array conversion. Please deserialize to JToken as cast it to JArray.");
            }

            var token = JsonDeserialize(data);

            if (token is JObject jobj) {
                return jobj.ToObject<T>();
            } else {
                return token.Value<T>();
            }
        }

        [CanBeNull]
        public static JToken JsonDeserialize([NotNull] string str) {
            var bytes = Utf8WithoutBom.GetBytes(str);
            return JsonDeserialize(bytes);
        }

        [CanBeNull]
        public static JToken JsonDeserialize([NotNull] byte[] data) {
            JToken result;

            using (var memoryStream = new MemoryStream(data, false)) {
                using (var textReader = new StreamReader(memoryStream, Utf8WithoutBom)) {
                    using (var jsonReader = new JsonTextReader(textReader)) {
                        result = JsonSerializer.Value.Deserialize(jsonReader) as JToken;
                    }
                }
            }

            return result;
        }

        internal static readonly Lazy<JsonSerializer> JsonSerializer = new Lazy<JsonSerializer>(() => new JsonSerializer());

    }
}
