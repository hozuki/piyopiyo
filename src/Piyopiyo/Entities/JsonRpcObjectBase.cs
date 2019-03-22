using JetBrains.Annotations;
using Newtonsoft.Json;

namespace OpenMLTD.Piyopiyo.Entities {
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JsonRpcObjectBase {

        public const string CurrentVersion = "2.0";

        protected JsonRpcObjectBase() {
            Version = string.Empty;
        }

        [JsonProperty("jsonrpc")]
        [NotNull]
        public string Version { get; protected set; }

    }
}
