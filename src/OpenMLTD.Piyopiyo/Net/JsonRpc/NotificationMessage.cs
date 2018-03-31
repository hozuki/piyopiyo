using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public sealed class NotificationMessage : Message {

        [JsonConstructor]
        private NotificationMessage() {
        }

        [JsonProperty]
        [NotNull]
        public string Method { get; set; }

        [JsonProperty]
        [NotNull, ItemCanBeNull]
        public JArray Params { get; set; }

        [NotNull]
        public static NotificationMessage FromParams([NotNull] string method, [CanBeNull, ItemCanBeNull] IEnumerable paramList) {
            var message = new NotificationMessage {
                Params = new JArray(),
                Method = method,
            };

            if (paramList != null) {
                foreach (var param in paramList) {
                    message.Params.Add(JToken.FromObject(param));
                }
            }

            return message;
        }

        /// <summary>
        /// Creates a <see cref="RequestMessage"/> from a given RPC method name, a params object, and an optional command ID.
        /// Please note that the declaration order of members of the param object reflects on the order of parameter array elements.
        /// </summary>
        /// <param name="method">The RPC method name.</param>
        /// <param name="paramListObject">An object containing parameters.</param>
        /// <returns></returns>
        [NotNull]
        public static NotificationMessage FromParamObject([NotNull] string method, [CanBeNull] object paramListObject) {
            List<JToken> paramList;

            if (paramListObject == null) {
                paramList = null;
            } else {
                var jobject = JObject.FromObject(paramListObject, BvspHelper.JsonSerializer.Value);

                paramList = new List<JToken>();

                foreach (var property in jobject.Properties()) {
                    paramList.Add(property.Value);
                }
            }

            var message = new NotificationMessage {
                Params = new JArray(),
                Method = method,
            };

            if (paramList != null) {
                foreach (var param in paramList) {
                    message.Params.Add(param);
                }
            }

            return message;
        }

    }
}
