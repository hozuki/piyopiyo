using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace OpenMLTD.Piyopiyo.Net {
    public sealed class JsonRpcCallResult {

        internal JsonRpcCallResult(HttpStatusCode statusCode, [CanBeNull] JToken responseObject) {
            StatusCode = statusCode;
            ResponseObject = responseObject;
        }

        public HttpStatusCode StatusCode { get; }

        [CanBeNull]
        public JToken ResponseObject { get; }

    }
}
