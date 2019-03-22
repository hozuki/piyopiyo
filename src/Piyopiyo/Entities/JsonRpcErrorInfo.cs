using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenMLTD.Piyopiyo.Entities {
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class JsonRpcErrorInfo {

        [JsonConstructor]
        public JsonRpcErrorInfo() {
            Message = string.Empty;
        }

        [JsonProperty("code", Required = Required.DisallowNull)]
        public int Code {
            [DebuggerStepThrough]
            get;
            [DebuggerStepThrough]
            set;
        }

        [JsonProperty("message", Required = Required.DisallowNull)]
        [NotNull]
        public string Message {
            [DebuggerStepThrough]
            get;
            [DebuggerStepThrough]
            set;
        }

        [JsonProperty("data")]
        [CanBeNull]
        private JObject Data {
            [DebuggerStepThrough]
            get;
            [DebuggerStepThrough]
            set;
        }

        [CanBeNull]
        public object GetData([NotNull] Type objectType) {
            return Data?.ToObject(objectType, Utilities.Serializer);
        }

        [CanBeNull]
        public T GetData<T>() {
            var data = Data;

            if (data.IsNull()) {
                var t = typeof(T);

                if (t.IsValueType) {
                    throw new InvalidCastException("Cannot assign null value to a value type.");
                }

                return default;
            }

            return data.ToObject<T>(Utilities.Serializer);
        }

    }
}
