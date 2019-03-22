using System.Diagnostics;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenMLTD.Piyopiyo.Entities {
    [JsonObject(MemberSerialization.OptIn)]
    public class JsonRpcResponse : JsonRpcResponseBase {

        [JsonConstructor]
        public JsonRpcResponse() {
        }

        [JsonProperty("result")]
        [CanBeNull]
        public JToken Result {
            [DebuggerStepThrough]
            get;
            [DebuggerStepThrough]
            set;
        }

        [NotNull]
        internal static JsonRpcResponse CreateEmpty() {
            return new JsonRpcResponse {
                Version = CurrentVersion
            };
        }

    }
}
