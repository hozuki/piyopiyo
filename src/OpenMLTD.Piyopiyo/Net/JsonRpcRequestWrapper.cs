using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net {
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public sealed class JsonRpcRequestWrapper {

        [JsonConstructor]
        private JsonRpcRequestWrapper() {
        }

        [NotNull]
        public static JsonRpcRequestWrapper FromParams([NotNull] string method, [CanBeNull, ItemCanBeNull] IEnumerable paramList, [CanBeNull] string id = null) {
            var wrapper = new JsonRpcRequestWrapper {
                Params = new JArray(),
                Method = method,
                Id = id
            };

            if (paramList != null) {
                foreach (var param in paramList) {
                    wrapper.Params.Add(JToken.FromObject(param));
                }
            }

            return wrapper;
        }

        [CanBeNull]
        [ContractAnnotation("paramArray:null => null")]
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
        [ContractAnnotation("paramArray:null => null")]
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

        [JsonProperty(PropertyName = "jsonrpc")]
        [NotNull]
        public string JsonRpcVersion { get; set; } = "2.0";

        [JsonProperty]
        [NotNull]
        public string Method { get; set; }

        [JsonProperty]
        [NotNull, ItemCanBeNull]
        public JArray Params { get; set; }

        [JsonProperty]
        [CanBeNull]
        public object Id { get; set; }

        private struct PropertyOrField {

            internal PropertyOrField([NotNull] PropertyInfo property) {
                _property = property;
                _field = null;
            }

            internal PropertyOrField([NotNull] FieldInfo field) {
                _property = null;
                _field = field;
            }

            [CanBeNull]
            internal T GetCustomAttribute<T>()
                where T : Attribute {
                if (_property != null) {
                    return _property.GetCustomAttribute<T>();
                } else if (_field != null) {
                    return _field.GetCustomAttribute<T>();
                } else {
                    return null;
                }
            }

            internal void SetValue([NotNull] object obj, [CanBeNull] object value) {
                if (_property != null) {
                    _property.SetValue(obj, value);
                } else if (_field != null) {
                    _field.SetValue(obj, value);
                }
            }

            [CanBeNull]
            internal Type GetUnderlyingType() {
                if (_property != null) {
                    return _property.PropertyType;
                } else if (_field != null) {
                    return _field.FieldType;
                } else {
                    return null;
                }
            }

            [CanBeNull]
            private readonly PropertyInfo _property;
            [CanBeNull]
            private readonly FieldInfo _field;

        }

    }
}
