using System;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    public sealed class ResponseMessageContractResolver : PiyopiyoContractResolver {

        public static ResponseMessageContractResolver Instance {
            get {
                if (_instance == null) {
                    _instance = new ResponseMessageContractResolver();
                }

                return _instance;
            }
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(ResponseMessage)) {
                if (property.PropertyType == typeof(JToken) && property.PropertyName == Naming.GetPropertyName(nameof(ResponseMessage.Result), false)) {
                    property.ShouldSerialize = instance => {
                        var message = (ResponseMessage)instance;
                        return message.ShouldSerializeAsSuccessful;
                    };
                } else if (property.PropertyType == typeof(ResponseError) && property.PropertyName == Naming.GetPropertyName(nameof(ResponseMessage.Error), false)) {
                    property.ShouldSerialize = instance => {
                        var message = (ResponseMessage)instance;
                        return !message.ShouldSerializeAsSuccessful;
                    };
                }
            }

            return property;
        }

        [CanBeNull]
        [ThreadStatic]
        private static ResponseMessageContractResolver _instance;

    }
}
