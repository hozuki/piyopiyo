using System.Collections.Generic;
using System.Net.Http;
using HTTPnet.Core.Http.Raw;
using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Entities;

namespace OpenMLTD.Piyopiyo.Rpc.Implementation {
    internal sealed class RpcSessionRequest : IRpcRequest {

        public RpcSessionRequest([NotNull] RpcSessionContext context) {
            Context = context;
            RawRequest = context.RawContext.HttpContext.Request;
        }

        [NotNull]
        public RpcSessionContext Context { get; }

        [NotNull]
        public RawHttpRequest RawRequest { get; }

        public HttpMethod Method {
            get {
                var method = RawRequest.Method;

                return new HttpMethod(method);
            }
        }

        public IReadOnlyDictionary<string, string> Headers => RawRequest.Headers;

        public JsonRpcRequestBase GetRequestBody() {
            return Context.GetRequestBodyAsJsonRpc();
        }

    }
}
