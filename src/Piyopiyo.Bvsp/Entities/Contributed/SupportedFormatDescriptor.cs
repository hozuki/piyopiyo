using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace OpenMLTD.Piyopiyo.Bvsp.Entities.Contributed {
    [JsonObject(MemberSerialization.OptIn)]
    public class SupportedFormatDescriptor {

        [JsonConstructor]
        public SupportedFormatDescriptor() {
            GameId = string.Empty;
            FormatId = string.Empty;
            Versions = Array.Empty<string>();
        }

        [JsonProperty("game", Required = Required.DisallowNull)]
        [NotNull]
        public string GameId { get; set; }

        [JsonProperty("id", Required = Required.DisallowNull)]
        [NotNull]
        public string FormatId { get; set; }

        [JsonProperty("versions", Required = Required.DisallowNull)]
        [NotNull]
        [ItemNotNull]
        public string[] Versions { get; set; }

        [JsonProperty("extra")]
        [CanBeNull]
        public object Extra { get; set; }

    }
}
