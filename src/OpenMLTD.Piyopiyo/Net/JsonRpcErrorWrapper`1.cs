using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net {
    /// <summary>
    /// Used for responses with a single result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public sealed class JsonRpcErrorWrapper<T> : JsonRpcResponseWrapper {

        [JsonConstructor]
        internal JsonRpcErrorWrapper() {
        }

        [JsonProperty]
        [NotNull]
        public ErrorInfo Error { get; internal set; }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public sealed class ErrorInfo {

            [JsonConstructor]
            internal ErrorInfo() {
            }

            [JsonProperty]
            public int Code { get; internal set; }

            [JsonProperty]
            [NotNull]
            public string Message { get; internal set; }

            [JsonProperty]
            [CanBeNull]
            public T Data { get; internal set; }

        }

    }
}
