using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net {
    /// <summary>
    /// Used for responses with a single result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public sealed class JsonRpcResponseWrapper<T> : JsonRpcResponseWrapper {

        [JsonConstructor]
        internal JsonRpcResponseWrapper() {
        }

        [CanBeNull]
        public T Result { get; set; }

    }
}
