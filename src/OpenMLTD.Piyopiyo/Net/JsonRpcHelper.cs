using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace OpenMLTD.Piyopiyo.Net {
    public static class JsonRpcHelper {

        public static bool IsRequestValid([NotNull] JToken obj) {
            return IsRequestValid(obj, out IReadOnlyList<string> _);
        }

        public static bool IsRequestValid([NotNull] JToken obj, [NotNull] out string errorMessage) {
            var b = IsRequestValid(obj, out IReadOnlyList<string> messages);

            errorMessage = string.Join("\n", messages);

            return b;
        }

        public static bool IsRequestValid([NotNull] JToken obj, [NotNull, ItemNotNull] out IReadOnlyList<string> messages) {
            if (_requestSchema == null) {
                _requestSchema = JSchema.Parse(JsonRpcRequestSchema);
            }

            var b = obj.IsValid(_requestSchema, out IList<string> errors);

            messages = errors.ToArray();

            return b;
        }

        public static bool IsResponseValid([NotNull] JToken obj) {
            return IsResponseValid(obj, out IReadOnlyList<string> _);
        }

        public static bool IsResponseValid([NotNull] JToken obj, [NotNull] out string errorMessage) {
            var b = IsResponseValid(obj, out IReadOnlyList<string> messages);

            errorMessage = string.Join("\n", messages);

            return b;
        }

        public static bool IsResponseValid([NotNull] JToken obj, [NotNull, ItemNotNull] out IReadOnlyList<string> messages) {
            //if (_responseSchema == null) {
            //    _responseSchema = JSchema.Parse(JsonRpcResponseSchema);
            //}

            //var b = obj.IsValid(_responseSchema, out IList<string> errors);

            //messages = errors.ToArray();

            //return b;

            // TODO: Sadly, Json.NET does not support a full functionality of JSON schemas. See https://github.com/JamesNK/Newtonsoft.Json.Schema/issues/132.
            // This error causes Json.NET failing to validate the response object. Verify it here: https://www.jsonschemavalidator.net/
            // So we have to ignore that and directly return true, for now.

            messages = EmptyStringArray;

            return true;
        }

        public static bool IsResponseSuccessful([NotNull] JToken token) {
            if (!IsResponseValid(token, out string errorMessage)) {
                throw new FormatException("The response object is not a valid JSON RPC 2.0 respose object:\n" + errorMessage);
            }

            if (token["result"] != null) {
                return true;
            } else if (token["error"] != null) {
                return false;
            } else {
                throw new FormatException("The response object does not conform JSON RPC 2.0 specification.");
            }
        }

        [NotNull]
        public static JsonRpcErrorWrapper<TErrorData> TranslateAsError<TErrorData>([NotNull] JToken token) {
            if (!IsResponseValid(token, out string errorMessage)) {
                throw new FormatException("The response object is not a valid JSON RPC 2.0 respose object:\n" + errorMessage);
            }

            if (token["error"] != null) {
                var result = token.ToObject<JsonRpcErrorWrapper<TErrorData>>();
                return result;
            } else {
                throw new FormatException("The response object does not contain a valid error object. Maybe it contains a response object.");
            }
        }

        [NotNull]
        public static JsonRpcErrorWrapper<object> TranslateAsError([NotNull] JToken token) {
            return TranslateAsError<object>(token);
        }

        [NotNull]
        public static JsonRpcResponseWrapper<TResult> TranslateAsResponse<TResult>([NotNull] JToken token) {
            if (!IsResponseValid(token, out string errorMessage)) {
                throw new FormatException("The response object is not a valid JSON RPC 2.0 respose object:\n" + errorMessage);
            }

            if (token["result"] != null) {
                var result = token.ToObject<JsonRpcResponseWrapper<TResult>>();
                return result;
            } else {
                throw new FormatException("The response object does not contain a valid response object. Maybe it contains an error object.");
            }
        }

        // https://github.com/fge/sample-json-schemas/tree/master/jsonrpc2.0
        private const string JsonRpcRequestSchema = @"{
    ""$schema"": ""http://json-schema.org/draft-04/schema#"",
    ""description"": ""A JSON RPC 2.0 request"",
    ""oneOf"": [
        {
            ""description"": ""An individual request"",
            ""$ref"": ""#/definitions/request""
        },
        {
            ""description"": ""An array of requests"",
            ""type"": ""array"",
            ""items"": { ""$ref"": ""#/definitions/request"" }
        }
    ],
    ""definitions"": {
        ""request"": {
            ""type"": ""object"",
            ""required"": [ ""jsonrpc"", ""method"" ],
            ""properties"": {
                ""jsonrpc"": { ""enum"": [ ""2.0"" ] },
                ""method"": {
                    ""type"": ""string""
                },
                ""id"": {
                    ""type"": [ ""string"", ""number"", ""null"" ],
                    ""note"": [
                        ""While allowed, null should be avoided: http://www.jsonrpc.org/specification#id1"",
                        ""While allowed, a number with a fractional part should be avoided: http://www.jsonrpc.org/specification#id2""
                    ]
                },
                ""params"": {
                    ""type"": [ ""array"", ""object"" ]
                }
            }
        }
    }
}";

        // https://github.com/fge/sample-json-schemas/tree/master/jsonrpc2.0
        private const string JsonRpcResponseSchema = @"{
    ""$schema"": ""http://json-schema.org/draft-04/schema#"",
    ""description"": ""A JSON RPC 2.0 response"",
    ""oneOf"": [
        { ""$ref"": ""#/definitions/success"" },
        { ""$ref"": ""#/definitions/error"" },
        {
            ""type"": ""array"",
            ""items"": {
                ""oneOf"": [
                    { ""$ref"": ""#/definitions/success"" },
                    { ""$ref"": ""#/definitions/error"" }
                ]
            }
        }
    ],
    ""definitions"": {
        ""common"": {
            ""required"": [ ""id"", ""jsonrpc"" ],
            ""not"": {
                ""description"": ""cannot have result and error at the same time"",
                ""required"": [ ""result"", ""error"" ]
            },
            ""type"": ""object"",
            ""properties"": {
                ""id"": {
                    ""type"": [ ""string"", ""integer"", ""null"" ],
                    ""note"": [
                        ""spec says a number which should not contain a fractional part"",
                        ""We choose integer here, but this is unenforceable with some languages""
                    ]
                },
                ""jsonrpc"": { ""enum"": [ ""2.0"" ] }
            }
        },
        ""success"": {
            ""description"": ""A success. The result member is then required and can be anything."",
            ""allOf"": [
                { ""$ref"": ""#/definitions/common"" },
                { ""required"": [ ""result"" ] }
            ]
        },
        ""error"": {
            ""allOf"" : [
                { ""$ref"": ""#/definitions/common"" },
                {
                    ""required"": [ ""error"" ],
                    ""properties"": {
                        ""error"": {
                            ""type"": ""object"",
                            ""required"": [ ""code"", ""message"" ],
                            ""properties"": {
                                ""code"": {
                                    ""type"": ""integer"",
                                    ""note"": [ ""unenforceable in some languages"" ]
                                },
                                ""message"": { ""type"": ""string"" },
                                ""data"": {
                                    ""description"": ""optional, can be anything""
                                }
                            }
                        }
                    }
                }
            ]
        }
    }
}";

        private static readonly string[] EmptyStringArray = new string[0];

        private static JSchema _requestSchema;
        private static JSchema _responseSchema;

    }
}
