using JetBrains.Annotations;
using Newtonsoft.Json;
using OpenMLTD.Piyopiyo.Bvsp.Entities.Contributed;

namespace OpenMLTD.Piyopiyo.Bvsp.Entities {
    [JsonObject(MemberSerialization.OptIn)]
    public class SimInitializeResult {

        [JsonProperty("supported_format")]
        [CanBeNull]
        public SelectedFormatDescriptor SelectedFormat { get; set; }

    }
}
