using System;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    public sealed class RequestMessageContractResolver : PiyopiyoContractResolver {

        public static RequestMessageContractResolver Instance {
            get {
                if (_instance == null) {
                    _instance = new RequestMessageContractResolver();
                }

                return _instance;
            }
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(RequestMessage) && property.PropertyType == typeof(JToken) && property.PropertyName == Naming.GetPropertyName(nameof(RequestMessage.Id), false)) {
                property.ShouldSerialize = instance => {
                    var message = (RequestMessage)instance;
                    return message.ShouldSerializeIdMember;
                };
            }

            return property;
        }

        [CanBeNull]
        [ThreadStatic]
        private static RequestMessageContractResolver _instance;

    }
}
