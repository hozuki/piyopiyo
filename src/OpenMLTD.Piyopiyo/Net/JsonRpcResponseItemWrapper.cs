using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net {
    /// <summary>
    /// Used for responses with multiple results for parsing responses from the official server.
    /// It maps to each array element in the response array.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public sealed class JsonRpcResponseItemWrapper {

        public static JsonRpcResponseItemWrapper FromResult([CanBeNull] object result) {
            return new JsonRpcResponseItemWrapper {
                Result = result
            };
        }

        [JsonProperty(PropertyName = "jsonrpc")]
        [NotNull]
        public string JsonRpcVersion { get; set; } = "2.0";

        /// <summary>
        /// This is a JToken (and usually a JObject) when it is used to obtain deserialized data from official response.
        /// When it is used in constructing response array to send a custom response, you should set this value to a custom
        /// object (NOT JObject). In the latter case, use <see cref="FromResult"/> as a shorthand.
        /// </summary>
        public object Result { get; set; }

        [JsonProperty(PropertyName = "id")]
        [CanBeNull]
        public object ID { get; set; }

    }
}
