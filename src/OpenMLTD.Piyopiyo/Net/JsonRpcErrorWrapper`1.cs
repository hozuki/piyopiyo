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

        [CanBeNull]
        public ErrorInfo Error { get; internal set; }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public sealed class ErrorInfo {

            internal ErrorInfo() {
            }

            public int Code { get; internal set; }

            [NotNull]
            public string Message { get; internal set; }

            [CanBeNull]
            public T Data { get; internal set; }

        }

    }
}
