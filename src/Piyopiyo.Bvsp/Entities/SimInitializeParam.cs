using JetBrains.Annotations;
using Newtonsoft.Json;
using OpenMLTD.Piyopiyo.Bvsp.Entities.Contributed;

namespace OpenMLTD.Piyopiyo.Bvsp.Entities {
    [JsonObject(MemberSerialization.OptIn)]
    public class SimInitializeParam {

        [JsonProperty("supported_formats")]
        [CanBeNull]
        [ItemNotNull]
        public SupportedFormatDescriptor[] SupportedFormats { get; set; }

    }
}
