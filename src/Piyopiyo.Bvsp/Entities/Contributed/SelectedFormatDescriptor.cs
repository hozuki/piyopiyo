using JetBrains.Annotations;
using Newtonsoft.Json;

namespace OpenMLTD.Piyopiyo.Bvsp.Entities.Contributed {
    [JsonObject(MemberSerialization.OptIn)]
    public class SelectedFormatDescriptor {

        [JsonConstructor]
        public SelectedFormatDescriptor() {
            GameId = string.Empty;
            FormatId = string.Empty;
            Version = string.Empty;
        }

        [JsonProperty("game", Required = Required.DisallowNull)]
        [NotNull]
        public string GameId { get; set; }

        [JsonProperty("id", Required = Required.DisallowNull)]
        [NotNull]
        public string FormatId { get; set; }

        [JsonProperty("versions", Required = Required.DisallowNull)]
        [NotNull]
        public string Version { get; set; }

        [JsonProperty("extra")]
        [CanBeNull]
        public object Extra { get; set; }

    }
}
