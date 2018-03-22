using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net {
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class JsonRpcResponseWrapper {

        protected JsonRpcResponseWrapper() {
        }

        [NotNull]
        public static JsonRpcResponseWrapper<T> FromResult<T>([CanBeNull] T result, [CanBeNull] string id = null) {
            return new JsonRpcResponseWrapper<T> {
                Result = result,
                Id = id
            };
        }

        [NotNull]
        public static JsonRpcErrorWrapper<T> FromError<T>(int code, [NotNull] string message, [CanBeNull] T data, [CanBeNull] string id = null) {
            return new JsonRpcErrorWrapper<T> {
                Error = new JsonRpcErrorWrapper<T>.ErrorInfo {
                    Code = code,
                    Message = message,
                    Data = data
                },
                Id = id
            };
        }

        [NotNull]
        public static JsonRpcErrorWrapper<object> FromError(int code, [NotNull] string message, [CanBeNull] string id = null) {
            return new JsonRpcErrorWrapper<object> {
                Error = new JsonRpcErrorWrapper<object>.ErrorInfo {
                    Code = code,
                    Message = message
                },
                Id = id
            };
        }

        [JsonProperty(PropertyName = "jsonrpc")]
        [NotNull]
        public string JsonRpcVersion { get; set; } = "2.0";

        [JsonProperty]
        [CanBeNull]
        public string Id { get; set; }

    }
}
