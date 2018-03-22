using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net {
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public sealed class JsonRpcRequestWrapper {

        [JsonConstructor]
        private JsonRpcRequestWrapper() {
            Params = new List<JToken>();
        }

        [NotNull]
        public static JsonRpcRequestWrapper FromParams([NotNull] string method, [NotNull, ItemCanBeNull] IEnumerable paramList, [CanBeNull] string id = null) {
            var wrapper = new JsonRpcRequestWrapper {
                Method = method,
                Id = id
            };

            foreach (var param in paramList) {
                wrapper.Params.Add(JToken.FromObject(param));
            }

            return wrapper;
        }

        [JsonProperty(PropertyName = "jsonrpc")]
        [NotNull]
        public string JsonRpcVersion { get; set; } = "2.0";

        [NotNull]
        public string Method { get; set; }

        [NotNull, ItemCanBeNull]
        public List<JToken> Params { get; }

        [CanBeNull]
        public object Id { get; set; }

    }
}
