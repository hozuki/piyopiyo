using System.Diagnostics;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace OpenMLTD.Piyopiyo.Entities {
    public sealed class JsonRpcError : JsonRpcResponseBase {

        [JsonConstructor]
        public JsonRpcError() {
        }

        [JsonProperty("error")]
        public JsonRpcErrorInfo Error {
            [DebuggerStepThrough]
            get;
            [DebuggerStepThrough]
            set;
        }

        [NotNull]
        public static JsonRpcError CreateEmpty() {
            return new JsonRpcError {
                Version = CurrentVersion,
                Error = new JsonRpcErrorInfo()
            };
        }

    }
}
