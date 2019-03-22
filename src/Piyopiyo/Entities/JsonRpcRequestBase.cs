using System.Diagnostics;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenMLTD.Piyopiyo.Entities {
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JsonRpcRequestBase : JsonRpcObjectBase {

        protected JsonRpcRequestBase() {
            Version = CurrentVersion;
            Method = string.Empty;
            Params = new JArray();
        }

        [JsonProperty("method", Required = Required.DisallowNull)]
        [NotNull]
        public string Method {
            [DebuggerStepThrough]
            get;
            [DebuggerStepThrough]
            set;
        }

        [JsonProperty("params", Required = Required.DisallowNull)]
        [NotNull]
        [ItemCanBeNull]
        public JArray Params {
            [DebuggerStepThrough]
            get;
            [DebuggerStepThrough]
            set;
        }

    }
}
