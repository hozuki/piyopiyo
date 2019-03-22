using System;
using System.Net;
using System.Net.Http;
using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Rpc;

namespace OpenMLTD.Piyopiyo.Bvsp.Client {
    public abstract class BvspServiceBase : RpcServiceBase {

        protected BvspServiceBase([NotNull] Uri serviceUri)
            : base(serviceUri) {
        }

        [NotNull]
        protected T CreateProxy<T>()
            where T : class {
            return CreateProxy<T>(CreateWebRequest);
        }

        [NotNull]
        private HttpWebRequest CreateWebRequest() {
            var r = WebRequest.CreateHttp(ServiceUri);

            r.Method = HttpMethod.Post.Method;
            r.Headers.Add("Content-Type", ProtocolConstants.ContentType);

            return r;
        }

    }
}
