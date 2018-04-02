using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenMLTD.Piyopiyo.Net.JsonRpc;

namespace OpenMLTD.Piyopiyo {
    public static class BvspHelper {

        public const string BvspContentType = "application/jsonrpc-bvsp";
        public const string BvspCharSet = "utf-8";

        [NotNull]
        public static readonly byte[] EmptyBytes = new byte[0];

        [NotNull]
        public static readonly Encoding Utf8WithoutBom = new UTF8Encoding(false);

        /// <summary>
        /// Creates a <see cref="JToken"/> from given <see cref="object"/>.
        /// This method wraps a layer for <see langword="null"/> value, which throws <see cref="ArgumentNullException"/> when passing to <see cref="JToken.FromObject(object)"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <remarks>
        /// Json.NET should have handled this value properly.
        /// </remarks>
        /// <returns>Created <see cref="JToken"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NotNull]
        public static JToken CreateJToken([CanBeNull] object obj) {
            if (ReferenceEquals(obj, null)) {
                return JValue.CreateNull();
            } else {
                return JToken.FromObject(obj);
            }
        }

        /// <summary>
        /// Creates a <see cref="JToken"/> from given <see cref="object"/>.
        /// This method wraps a layer for <see langword="null"/> value, which throws <see cref="ArgumentNullException"/> when passing to <see cref="JToken.FromObject(object, JsonSerializer)"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="serializer">The <see cref="JsonSerializer"/> to use.</param>
        /// <remarks>
        /// Json.NET should have handled this value properly.
        /// </remarks>
        /// <returns>Created <see cref="JToken"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NotNull]
        public static JToken CreateJToken([CanBeNull] object obj, [NotNull] JsonSerializer serializer) {
            if (ReferenceEquals(obj, null)) {
                return JValue.CreateNull();
            } else {
                return JToken.FromObject(obj, serializer);
            }
        }

        /// <summary>
        /// Creates a <see cref="JObject"/> from given <see cref="object"/>.
        /// This method wraps a layer for <see langword="null"/> value, which throws <see cref="ArgumentNullException"/> when passing to <see cref="JObject.FromObject(object)"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <remarks>
        /// Json.NET should have handled this value properly.
        /// </remarks>
        /// <returns>Created <see cref="JObject"/>, or <see langword="null"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [CanBeNull]
        [ContractAnnotation("obj:null => null; obj:notnull => notnull")]
        public static JObject CreateJObject([CanBeNull] object obj) {
            if (ReferenceEquals(obj, null)) {
                return null;
            } else {
                return JObject.FromObject(obj);
            }
        }

        /// <summary>
        /// Creates a <see cref="JObject"/> from given <see cref="object"/>.
        /// This method wraps a layer for <see langword="null"/> value, which throws <see cref="ArgumentNullException"/> when passing to <see cref="JObject.FromObject(object, JsonSerializer)"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="serializer">The <see cref="JsonSerializer"/> to use.</param>
        /// <remarks>
        /// Json.NET should have handled this value properly.
        /// </remarks>
        /// <returns>Created <see cref="JObject"/>, or <see langword="null"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [CanBeNull]
        [ContractAnnotation("obj:null => null; obj:notnull => notnull")]
        public static JObject CreateJObject([CanBeNull] object obj, [NotNull] JsonSerializer serializer) {
            if (ReferenceEquals(obj, null)) {
                return null;
            } else {
                return JObject.FromObject(obj, serializer);
            }
        }

        [NotNull]
        public static string JsonSerializeToString([NotNull] object obj, [NotNull] JsonSerializer serializer) {
            var bytes = JsonSerializeToByteArray(obj, serializer);
            return Utf8WithoutBom.GetString(bytes);
        }

        [NotNull]
        public static byte[] JsonSerializeToByteArray([NotNull] object obj, [NotNull] JsonSerializer serializer) {
            byte[] result;

            using (var memoryStream = new MemoryStream()) {
                using (var textWriter = new StreamWriter(memoryStream, Utf8WithoutBom)) {
                    serializer.Serialize(textWriter, obj);
                }

                result = memoryStream.ToArray();
            }

            return result;
        }

        [NotNull]
        public static string JsonSerializeRequestToString([NotNull] object obj) {
            return JsonSerializeToString(obj, RequestJsonSerializer);
        }

        [NotNull]
        public static string JsonSerializeResponseToString([NotNull] object obj) {
            return JsonSerializeToString(obj, ResponseJsonSerializer);
        }

        [CanBeNull]
        public static T JsonDeserialize<T>([NotNull] string str, [NotNull] JsonSerializer serializer) {
            var bytes = Utf8WithoutBom.GetBytes(str);
            return JsonDeserialize<T>(bytes, serializer);
        }

        [CanBeNull]
        public static T JsonDeserialize<T>([NotNull] byte[] data, [NotNull] JsonSerializer serializer) {
            var t = typeof(T);

            if (t.IsArray) {
                throw new NotSupportedException("This method does not support array conversion. Please deserialize to JToken as cast it to JArray.");
            }

            var token = JsonDeserialize(data, serializer);

            if (token is JObject jobj) {
                return jobj.ToObject<T>();
            } else {
                return token.Value<T>();
            }
        }

        [CanBeNull]
        public static JToken JsonDeserialize([NotNull] string str, [NotNull] JsonSerializer serializer) {
            var bytes = Utf8WithoutBom.GetBytes(str);
            return JsonDeserialize(bytes, serializer);
        }

        [CanBeNull]
        public static JToken JsonDeserialize([NotNull] byte[] data, [NotNull] JsonSerializer serializer) {
            JToken result;

            using (var memoryStream = new MemoryStream(data, false)) {
                using (var textReader = new StreamReader(memoryStream, Utf8WithoutBom)) {
                    using (var jsonReader = new JsonTextReader(textReader)) {
                        result = serializer.Deserialize(jsonReader) as JToken;
                    }
                }
            }

            return result;
        }

        [CanBeNull]
        public static JToken JsonDeserialize([NotNull] string str) {
            var bytes = Utf8WithoutBom.GetBytes(str);
            return JsonDeserialize(bytes);
        }

        [CanBeNull]
        public static JToken JsonDeserialize([NotNull] byte[] data) {
            return JsonDeserialize(data, DefaultJsonSerializer);
        }

        [CanBeNull]
        [ContractAnnotation("paramArray:null => null; paramArray:notnull => notnull")]
        public static T ParamArrayToObject<T>([CanBeNull] JArray paramArray) {
            var t = typeof(T);
            var r = ParamArrayToObject(paramArray, t);

            return (T)r;
        }

        /// <summary>
        /// Deserailizes a parameter array to an entity class.
        /// Please note that the declaring order of members in the entity class DOES matter.
        /// Since we are going to deserialize duck-typed objects from a JSON array, we have to use the order of members to infer the order of array elements.
        /// Also, fields are handled after all properties so please do not mix property declaration and field declaration together.
        /// </summary>
        /// <param name="paramArray">The JSON RPC parameter array, which is the "params" member.</param>
        /// <param name="objectType">Type of the entity class.</param>
        /// <returns>Deserialized object.</returns>
        [CanBeNull]
        [ContractAnnotation("paramArray:null => null; paramArray:notnull => notnull")]
        public static object ParamArrayToObject([CanBeNull] JArray paramArray, [NotNull] Type objectType) {
            if (ReferenceEquals(paramArray, null)) {
                return TypeHelper.Default(objectType);
            }

            // TODO: This will only handle classes with the default constructor (new()), and structs. Classes without a default constructor will throw exception at this line.
            var obj = Activator.CreateInstance(objectType);

            const bool serializeCompilerGeneratedFields = false;

            var arrayLength = paramArray.Count;

            if (arrayLength > 0) {
                var jsonObjectAttribute = objectType.GetCustomAttribute<JsonObjectAttribute>();
                MemberSerialization memberSerialization;

                if (jsonObjectAttribute == null) {
                    memberSerialization = MemberSerialization.OptOut;
                } else {
                    memberSerialization = jsonObjectAttribute.MemberSerialization;
                }

                BindingFlags bindingFlags;
                bool serializeUnlessIgnored;

                var handlingItemIndex = 0;

                switch (memberSerialization) {
                    case MemberSerialization.OptOut:
                        bindingFlags = BindingFlags.Public | BindingFlags.Instance;
                        serializeUnlessIgnored = true;
                        break;
                    case MemberSerialization.OptIn:
                        bindingFlags = BindingFlags.Public | BindingFlags.Instance;
                        serializeUnlessIgnored = false;
                        break;
                    case MemberSerialization.Fields:
                        bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                        serializeUnlessIgnored = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var properties = objectType.GetProperties(bindingFlags);
                var fields = objectType.GetFields(bindingFlags);

                var properyOrFields = new PropertyOrField[properties.Length + fields.Length];

                for (var i = 0; i < properties.Length; ++i) {
                    properyOrFields[i] = new PropertyOrField(properties[i]);
                }

                for (var i = 0; i < fields.Length; ++i) {
                    properyOrFields[i + properties.Length] = new PropertyOrField(fields[i]);
                }

                foreach (var propertyOrField in properyOrFields) {
                    if (handlingItemIndex >= arrayLength) {
                        break;
                    }

                    if (!serializeCompilerGeneratedFields) {
                        var compilerGeneratedAttribute = propertyOrField.GetCustomAttribute<CompilerGeneratedAttribute>();

                        if (compilerGeneratedAttribute != null) {
                            continue;
                        }
                    }

                    var jsonIgnoreAttribute = propertyOrField.GetCustomAttribute<JsonIgnoreAttribute>();
                    var nonSerializedAttribute = propertyOrField.GetCustomAttribute<NonSerializedAttribute>();

                    if (jsonIgnoreAttribute != null || nonSerializedAttribute != null) {
                        continue;
                    }

                    if (!serializeUnlessIgnored) {
                        var jsonPropertyAttribute = propertyOrField.GetCustomAttribute<JsonPropertyAttribute>();
                        var dataMemberAttribute = propertyOrField.GetCustomAttribute<DataMemberAttribute>();

                        if (jsonPropertyAttribute == null && dataMemberAttribute == null) {
                            continue;
                        }
                    }

                    var underlyingType = propertyOrField.GetUnderlyingType();
                    var jsonToken = paramArray[handlingItemIndex];
                    var memberValue = jsonToken.ToObject(underlyingType);

                    propertyOrField.SetValue(obj, memberValue);

                    ++handlingItemIndex;
                }
            }

            return obj;
        }

        [NotNull]
        private static JsonSerializer RequestJsonSerializer {
            get {
                if (_requestJsonSerializer == null) {
                    _requestJsonSerializer = new JsonSerializer {
                        ContractResolver = new RequestMessageContractResolver()
                    };
                }

                return _requestJsonSerializer;
            }
        }

        [NotNull]
        private static JsonSerializer ResponseJsonSerializer {
            get {
                if (_responseJsonSerializer == null) {
                    _responseJsonSerializer = new JsonSerializer {
                        ContractResolver = new ResponseMessageContractResolver()
                    };
                }

                return _responseJsonSerializer;
            }
        }

        [NotNull]
        private static JsonSerializer DefaultJsonSerializer {
            get {
                if (_defaultJsonSerializer == null) {
                    _defaultJsonSerializer = new JsonSerializer();
                }

                return _defaultJsonSerializer;
            }
        }

        [ThreadStatic]
        [CanBeNull]
        private static JsonSerializer _requestJsonSerializer;

        [ThreadStatic]
        [CanBeNull]
        private static JsonSerializer _responseJsonSerializer;

        [ThreadStatic]
        [CanBeNull]
        private static JsonSerializer _defaultJsonSerializer;

    }
}
