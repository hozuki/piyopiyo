using JetBrains.Annotations;
using Newtonsoft.Json;

namespace OpenMLTD.Piyopiyo.Entities {
    [JsonObject(MemberSerialization.OptIn)]
    public class JsonRpcNotification : JsonRpcRequestBase {

        [JsonConstructor]
        public JsonRpcNotification() {
        }

        public JsonRpcNotification([NotNull] string method)
            : this() {
            Method = method;
        }

    }
}
