using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using NHttp;

namespace OpenMLTD.Piyopiyo.Net {
    public sealed class JsonRpcMethodEventArgs : EventArgs {

        internal JsonRpcMethodEventArgs([NotNull] HttpContext context, [NotNull] string parsedRequestBody, [NotNull] JObject parsedRequestObject) {
            Context = context;
            ParsedRequestBody = parsedRequestBody;
            ParsedRequestObject = parsedRequestObject;
        }

        [NotNull]
        public HttpContext Context { get; }

        [NotNull]
        public string ParsedRequestBody { get; }

        [NotNull]
        public JObject ParsedRequestObject { get; }

    }
}
