using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace OpenMLTD.Piyopiyo.Entities {
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JsonRpcResponseBase : JsonRpcObjectBase {

        private object _id;

        [JsonProperty("id")]
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
