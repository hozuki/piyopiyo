using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public sealed class ResponseMessage {

        [JsonConstructor]
        private ResponseMessage() {
        }

        /// <summary>
        /// Gets the request ID. <see langword="null"/> if this response is not triggered by a <see cref="RequestMessage"/>.
        /// </summary>
        [JsonProperty]
        [CanBeNull]
        public JToken Id { get; internal set; }

        /// <summary>
        /// Gets the error information. <see langword="null"/> if this response is a successful response.
        /// </summary>
        [JsonProperty]
        [CanBeNull]
        public ResponseError Error { get; internal set; }

        /// <summary>
        /// Gets the result object. <see langword="null"/> if this response is a failed response.
        /// </summary>
        [JsonProperty]
        [CanBeNull]
        public JToken Result { get; internal set; }

        /// <summary>
        /// Gets whether this response is successful.
        /// </summary>
        [JsonIgnore]
        public bool IsSuccessful {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Error == null;
        }

        [NotNull]
        public static ResponseMessage FromResult([CanBeNull] object result, [CanBeNull] string id) {
            var message = new ResponseMessage();

            message.Result = BvspHelper.CreateJToken(result);
            message.Id = id;
            message.ShouldSerializeAsSuccessful = true;

            return message;
        }

        [NotNull]
        public static ResponseMessage FromResult([CanBeNull] object result, int id) {
            var message = new ResponseMessage();

            message.Result = BvspHelper.CreateJToken(result);
            message.Id = id;
            message.ShouldSerializeAsSuccessful = true;

            return message;
        }

        [NotNull]
        public static ResponseMessage FromError<TErrorData>(int code, [NotNull] string message, [CanBeNull] TErrorData data, [CanBeNull] string id) {
            return new ResponseMessage {
                Error = new ResponseError {
                    Code = code,
                    Message = message,
                    Data = BvspHelper.CreateJToken(data)
                },
                Id = id,
                ShouldSerializeAsSuccessful = false
            };
        }

        [NotNull]
        public static ResponseMessage FromError<TErrorData>(int code, [NotNull] string message, [CanBeNull] TErrorData data, int id) {
            return new ResponseMessage {
                Error = new ResponseError {
                    Code = code,
                    Message = message,
                    Data = BvspHelper.CreateJToken(data)
                },
                Id = id,
                ShouldSerializeAsSuccessful = false
            };
        }

        [NotNull]
        public static ResponseMessage FromError(int code, [NotNull] string message, [CanBeNull] string id) {
            return new ResponseMessage {
                Error = new ResponseError {
                    Code = code,
                    Message = message
                },
                Id = id,
                ShouldSerializeAsSuccessful = false
            };
        }

        [NotNull]
        public static ResponseMessage FromError(int code, [NotNull] string message, int id) {
            return new ResponseMessage {
                Error = new ResponseError {
                    Code = code,
                    Message = message
                },
                Id = id,
                ShouldSerializeAsSuccessful = false
            };
        }

        /// <summary>
        /// Gets strong typed result object from <see cref="Result"/>.
        /// Returns <see langword="null"/> if the conversion fails.
        /// </summary>
        /// <typeparam name="T">Result object type.</typeparam>
        /// <returns>Strong typed result object.</returns>
        [CanBeNull]
        public T GetResult<T>() {
            var obj = GetResult(typeof(T));
            return (T)obj;
        }

        /// <summary>
        /// Gets strong typed result object from <see cref="Result"/>.
        /// Returns <see langword="null"/> if the conversion fails.
        /// </summary>
        /// <param name="resultType">Type of the result object.</param>
        /// <returns>Strong typed result object.</returns>
        [CanBeNull]
        public object GetResult([NotNull] Type resultType) {
            var canUseCache = _isResultDeserialized && resultType == _resultDeserializedType;

            if (!canUseCache) {
                if (Result == null) {
                    _deserializedResult = null;
                } else {
                    _deserializedResult = Result.ToObject(resultType);
                }

                _resultDeserializedType = resultType;
                _isResultDeserialized = true;
            }

            return _deserializedResult;
        }

        [JsonIgnore]
        internal bool ShouldSerializeAsSuccessful { get; set; }

        [CanBeNull]
        private object _deserializedResult;

        [CanBeNull]
        private Type _resultDeserializedType;

        private bool _isResultDeserialized;

    }
}
