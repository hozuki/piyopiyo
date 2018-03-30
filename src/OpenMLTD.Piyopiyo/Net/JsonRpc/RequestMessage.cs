using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public sealed class RequestMessage : Message {

        [JsonConstructor]
        private RequestMessage() {
        }

        [JsonProperty]
        [NotNull]
        public string Method { get; internal set; }

        [JsonProperty]
        [NotNull, ItemCanBeNull]
        public JArray Params { get; internal set; }

        [JsonProperty]
        [CanBeNull]
        public JToken Id { get; internal set; }

        [JsonIgnore]
        public bool HasId {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Id != null;
        }

        [JsonIgnore]
        public bool HasNonNullId {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Id != null && Id.Type != JTokenType.Null;
        }

        [NotNull]
        public static RequestMessage FromParams([NotNull] string method, [CanBeNull, ItemCanBeNull] IEnumerable paramList, [CanBeNull] string id) {
            var message = new RequestMessage {
                Params = new JArray(),
                Method = method,
                Id = id,
                ShouldSerializeIdMember = true
            };

            FillParamList(message, paramList);

            return message;
        }

        [NotNull]
        public static RequestMessage FromParams([NotNull] string method, [CanBeNull, ItemCanBeNull] IEnumerable paramList, int id) {
            var message = new RequestMessage {
                Params = new JArray(),
                Method = method,
                Id = id,
                ShouldSerializeIdMember = true
            };

            FillParamList(message, paramList);

            return message;
        }

        [NotNull]
        public static RequestMessage FromParams([NotNull] string method, [CanBeNull, ItemCanBeNull] IEnumerable paramList) {
            var message = new RequestMessage {
                Params = new JArray(),
                Method = method,
                ShouldSerializeIdMember = false
            };

            FillParamList(message, paramList);

            return message;
        }

        /// <summary>
        /// Creates a <see cref="RequestMessage"/> from a given RPC method name, a params object, and an optional command ID.
        /// The params object will be exploded to an array of parameters.
        /// Please note that the declaration order of members of the param object reflects on the order of parameter array elements.
        /// </summary>
        /// <param name="method">The RPC method name.</param>
        /// <param name="paramListObject">An object containing parameters.</param>
        /// <param name="id">Command ID. This parameter is optional.</param>
        /// <returns></returns>
        [NotNull]
        public static RequestMessage FromParamObject([NotNull] string method, [CanBeNull] object paramListObject, [CanBeNull] string id) {
            var paramList = BuildParamList(paramListObject);

            var message = new RequestMessage {
                Params = new JArray(),
                Method = method,
                Id = id,
                ShouldSerializeIdMember = true
            };

            FillParamList(message, paramList);

            return message;
        }

        [NotNull]
        public static RequestMessage FromParamObject([NotNull] string method, [CanBeNull] object paramListObject, int id) {
            var paramList = BuildParamList(paramListObject);

            var message = new RequestMessage {
                Params = new JArray(),
                Method = method,
                Id = id,
                ShouldSerializeIdMember = true
            };

            FillParamList(message, paramList);

            return message;
        }

        [NotNull]
        public static RequestMessage FromParamObject([NotNull] string method, [CanBeNull] object paramListObject) {
            var paramList = BuildParamList(paramListObject);

            var message = new RequestMessage {
                Params = new JArray(),
                Method = method,
                ShouldSerializeIdMember = false
            };

            FillParamList(message, paramList);

            return message;
        }

        internal bool ShouldSerializeIdMember { get; set; }

        [CanBeNull, ItemCanBeNull]
        private static IReadOnlyList<JToken> BuildParamList([CanBeNull] object paramValue) {
            List<JToken> paramList;

            if (paramValue == null) {
                paramList = null;
            } else {
                var jobject = JObject.FromObject(paramValue, DefaultJsonSerializer.Value);

                paramList = new List<JToken>();

                foreach (var property in jobject.Properties()) {
                    paramList.Add(property.Value);
                }
            }

            return paramList;
        }

        private static void FillParamList([NotNull] RequestMessage message, [CanBeNull] IEnumerable paramValues) {
            if (paramValues == null) {
                return;
            }

            foreach (var param in paramValues) {
                message.Params.Add(JToken.FromObject(param));
            }
        }

        private static void FillParamList([NotNull] RequestMessage message, [CanBeNull] IReadOnlyList<JToken> paramValues) {
            if (paramValues == null) {
                return;
            }

            foreach (var param in paramValues) {
                message.Params.Add(param);
            }
        }

        private static readonly Lazy<JsonSerializer> DefaultJsonSerializer = new Lazy<JsonSerializer>(() => new JsonSerializer());

    }
}
