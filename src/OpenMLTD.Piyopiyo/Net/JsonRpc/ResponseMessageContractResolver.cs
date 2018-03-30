using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    public sealed class ResponseMessageContractResolver : DefaultContractResolver {

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(ResponseMessage)) {
                if (property.PropertyType == typeof(JToken) && property.PropertyName == nameof(ResponseMessage.Result)) {
                    property.ShouldSerialize = instance => {
                        var message = (ResponseMessage)instance;
                        return message.ShouldSerializeAsSuccessful;
                    };
                } else if (property.PropertyType == typeof(ResponseError) && property.PropertyName == nameof(ResponseMessage.Error)) {
                    property.ShouldSerialize = instance => {
                        var message = (ResponseMessage)instance;
                        return !message.ShouldSerializeAsSuccessful;
                    };
                }
            }

            return property;
        }

    }
}
