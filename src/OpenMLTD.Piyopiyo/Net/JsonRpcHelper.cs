using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace OpenMLTD.Piyopiyo.Net {
    internal static class JsonRpcHelper {

        internal static bool IsRequestValid([NotNull] JObject obj) {
            return IsRequestValid(obj, out var _);
        }

        internal static bool IsRequestValid([NotNull] JObject obj, [NotNull, ItemNotNull] out IReadOnlyList<string> messages) {
            if (_requestSchema == null) {
                _requestSchema = JSchema.Parse(JsonRpcRequestSchema);
            }

            var b = obj.IsValid(_requestSchema, out IList<string> validatorMessages);

            messages = validatorMessages.ToArray();

            return b;
        }

        internal static bool IsResponseValid([NotNull] JObject obj) {
            return IsResponseValid(obj, out var _);
        }

        internal static bool IsResponseValid([NotNull] JObject obj, [NotNull, ItemNotNull] out IReadOnlyList<string> messages) {
            if (_responseSchema == null) {
                _responseSchema = JSchema.Parse(JsonRpcResponseSchema);
            }

            var b = obj.IsValid(_responseSchema, out IList<string> validatorMessages);

            messages = validatorMessages.ToArray();

            return b;
        }

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

        private static JSchema _requestSchema;
        private static JSchema _responseSchema;

    }
}
