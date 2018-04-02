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

        /// <summary>
        /// Gets RPC method name.
        /// </summary>
        [JsonProperty]
        [NotNull]
        public string Method { get; internal set; }

        /// <summary>
        /// Gets the <see cref="JArray"/> of prepared RPC parameters.
        /// </summary>
        [JsonProperty]
        [NotNull, ItemCanBeNull]
        public JArray Params { get; internal set; }

        /// <summary>
        /// Gets the request ID. This property is <see langword="null"/> if this <see cref="RequestMessage"/> is for a notification.
        /// </summary>
        [JsonProperty]
        [CanBeNull]
        public JToken Id { get; internal set; }

        /// <summary>
        /// Gets whether this <see cref="RequestMessage"/> has an ID.
        /// </summary>
        [JsonIgnore]
        public bool HasId {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Id != null;
        }

        /// <summary>
        /// Gets whether this <see cref="RequestMessage"/> has an ID whose value is null in JSON.
        /// </summary>
        [JsonIgnore]
        public bool HasNonNullId {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Id != null && Id.Type != JTokenType.Null;
        }

        /// <summary>
        /// Creates a <see cref="RequestMessage"/> for requests.
        /// </summary>
        /// <param name="method">RPC method.</param>
        /// <param name="paramList">List of parameters.</param>
        /// <param name="id">Request ID.</param>
        /// <returns>Created <see cref="RequestMessage"/>.</returns>
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

        /// <summary>
        /// Creates a <see cref="RequestMessage"/> for requests.
        /// </summary>
        /// <param name="method">RPC method.</param>
        /// <param name="paramList">List of parameters.</param>
        /// <param name="id">Request ID.</param>
        /// <returns>Created <see cref="RequestMessage"/>.</returns>
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

        /// <summary>
        /// Creates a <see cref="RequestMessage"/> for notifications.
        /// </summary>
        /// <param name="method">RPC method.</param>
        /// <param name="paramList">List of parameters.</param>
        /// <returns>Created <see cref="RequestMessage"/>.</returns>
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
        /// Creates a <see cref="RequestMessage"/> for requests from a given RPC method name, a params object, and an optional command ID.
        /// The params object will be exploded to an array of parameters.
        /// Please note that the declaration order of members of the param object reflects on the order of parameter array elements.
        /// </summary>
        /// <param name="method">The RPC method name.</param>
        /// <param name="paramListObject">An object containing parameters.</param>
        /// <param name="id">Request ID.</param>
        /// <returns>Created <see cref="RequestMessage"/>.</returns>
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

        /// <summary>
        /// Creates a <see cref="RequestMessage"/> for requests from a given RPC method name, a params object, and an optional command ID.
        /// The params object will be exploded to an array of parameters.
        /// Please note that the declaration order of members of the param object reflects on the order of parameter array elements.
        /// </summary>
        /// <param name="method">The RPC method name.</param>
        /// <param name="paramListObject">An object containing parameters.</param>
        /// <param name="id">Request ID.</param>
        /// <returns>Created <see cref="RequestMessage"/>.</returns>
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

        /// <summary>
        /// Creates a <see cref="RequestMessage"/> for notifications from a given RPC method name, a params object, and an optional command ID.
        /// The params object will be exploded to an array of parameters.
        /// Please note that the declaration order of members of the param object reflects on the order of parameter array elements.
        /// </summary>
        /// <param name="method">The RPC method name.</param>
        /// <param name="paramListObject">An object containing parameters.</param>
        /// <returns>Created <see cref="RequestMessage"/>.</returns>
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

        /// <summary>
        /// Gets/sets whether the <see cref="Id"/> property should be serialized in serialization.
        /// It should be <see langword="true"/> for requests, and <see langword="false"/> for notifications.
        /// </summary>
        internal bool ShouldSerializeIdMember { get; set; }

        [CanBeNull, ItemCanBeNull]
        private static IReadOnlyList<JToken> BuildParamList([CanBeNull] object paramValue) {
            List<JToken> paramList;

            if (paramValue == null) {
                paramList = null;
            } else {
                var jobject = BvspHelper.CreateJObject(paramValue, DefaultJsonSerializer.Value);

                paramList = new List<JToken>();

                foreach (var property in jobject.Properties()) {
                    paramList.Add(property.Value);
                }
            }

            return paramList;
        }

        private static void FillParamList([NotNull] RequestMessage message, [CanBeNull, ItemCanBeNull] IEnumerable paramValues) {
            if (paramValues == null) {
                return;
            }

            foreach (var param in paramValues) {
                message.Params.Add(BvspHelper.CreateJToken(param));
            }
        }

        private static void FillParamList([NotNull] RequestMessage message, [CanBeNull, ItemCanBeNull] IReadOnlyList<JToken> paramValues) {
            if (paramValues == null) {
                return;
            }

            foreach (var param in paramValues) {
                if (param == null) {
                    message.Params.Add(JValue.CreateNull());
                } else {
                    message.Params.Add(param);
                }
            }
        }

        private static readonly Lazy<JsonSerializer> DefaultJsonSerializer = new Lazy<JsonSerializer>(() => new JsonSerializer());

    }
}
