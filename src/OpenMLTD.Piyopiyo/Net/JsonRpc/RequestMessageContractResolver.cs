using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    internal sealed class RequestMessageContractResolver : DefaultContractResolver {

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(RequestMessage) && property.PropertyType == typeof(JToken) && property.PropertyName == nameof(RequestMessage.Id)) {
                property.ShouldSerialize = instance => {
                    var message = (RequestMessage)instance;
                    return message.ShouldSerializeIdMember;
                };
            }

            return property;
        }

    }
}
