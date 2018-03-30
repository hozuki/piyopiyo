using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Message {

        [JsonConstructor]
        internal Message() {
        }

        [JsonProperty("jsonrpc", Required = Required.Always)]
        [NotNull]
        public string JsonRpcVersion { get; set; } = "2.0";

    }
}
