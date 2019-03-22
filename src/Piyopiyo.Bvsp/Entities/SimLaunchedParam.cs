using JetBrains.Annotations;
using Newtonsoft.Json;

namespace OpenMLTD.Piyopiyo.Bvsp.Entities {
    [JsonObject(MemberSerialization.OptIn)]
    public class SimLaunchedParam {

        [JsonConstructor]
        public SimLaunchedParam() {
            ServerUri = string.Empty;
        }

        [JsonProperty("server_uri", Required = Required.DisallowNull)]
        [NotNull]
        public string ServerUri { get; set; }

    }
}
