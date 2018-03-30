using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public sealed class ResponseError {

        [JsonConstructor]
        internal ResponseError() {
        }

        [JsonProperty]
        public int Code { get; internal set; }

        [JsonProperty]
        [NotNull]
        public string Message { get; internal set; }

        [JsonProperty]
        [CanBeNull]
        public JToken Data { get; internal set; }

    }
}
