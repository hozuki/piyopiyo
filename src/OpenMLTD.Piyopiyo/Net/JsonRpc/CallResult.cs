using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    public sealed class CallResult {

        internal CallResult(HttpStatusCode statusCode, [CanBeNull] JToken responseObject) {
            StatusCode = statusCode;
            ResponseObject = responseObject;
        }

        public HttpStatusCode StatusCode { get; }

        [CanBeNull]
        public JToken ResponseObject { get; }

    }
}
