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
        internal ResponseMessage() {
        }

        [JsonProperty]
        [CanBeNull]
        public JToken Id { get; internal set; }

        [JsonProperty]
        [CanBeNull]
        public ResponseError Error { get; internal set; }

        [JsonProperty]
        [CanBeNull]
        public JToken Result { get; internal set; }

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

        [CanBeNull]
        public T GetResult<T>() {
            var obj = GetResult(typeof(T));
            return (T)obj;
        }

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

        internal bool ShouldSerializeAsSuccessful { get; set; }

        [CanBeNull]
        private object _deserializedResult;

        [CanBeNull]
        private Type _resultDeserializedType;

        private bool _isResultDeserialized;

    }
}
