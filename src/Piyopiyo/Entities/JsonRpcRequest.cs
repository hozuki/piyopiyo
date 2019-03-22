using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace OpenMLTD.Piyopiyo.Entities {
    [JsonObject(MemberSerialization.OptIn)]
    public class JsonRpcRequest : JsonRpcRequestBase {

        private object _id;

        [JsonConstructor]
        public JsonRpcRequest() {
        }

        public JsonRpcRequest([NotNull] string method)
            : this() {
            Method = method;
        }

        [JsonProperty("id", Required = Required.AllowNull)]
        [CanBeNull]
        public object Id {
            [DebuggerStepThrough]
            get => _id;
            set {
                if (ReferenceEquals(value, null)) {
                    _id = null;
                } else {
                    var valueType = value.GetType();

                    if (valueType != typeof(int) && valueType != typeof(string)) {
                        throw new ArgumentException("Only integer or string (including null) allowed.");
                    }

                    _id = value;
                }
            }
        }

    }
}
