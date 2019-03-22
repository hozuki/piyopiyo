using HTTPnet.Core.Pipeline;
using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Entities;

namespace OpenMLTD.Piyopiyo.Rpc.Implementation {
    internal sealed class RpcSessionContext : IRpcSessionContext {

        [NotNull]
        private const string BodyKey = "jrpc-request-body";

        // ReSharper disable once NotNullMemberIsNotInitialized
        private RpcSessionContext([NotNull] HttpContextPipelineHandlerContext context) {
            RawContext = context;
        }

        [NotNull]
        public HttpContextPipelineHandlerContext RawContext { get; }

        public IRpcRequest Request { get; private set; }

        public IRpcResponse Response { get; private set; }

        public void SaveRequestBody([NotNull] JsonRpcRequestBase body) {
            RawContext.Properties[BodyKey] = body;
        }

        [NotNull]
        public JsonRpcRequestBase GetRequestBodyAsJsonRpc() {
            return (JsonRpcRequestBase)RawContext.Properties[BodyKey];
        }

        [NotNull]
        internal static RpcSessionContext Wrap([NotNull] HttpContextPipelineHandlerContext context) {
            var c = new RpcSessionContext(context);

            var request = new RpcSessionRequest(c);
            var response = new RpcSessionResponse(c);

            c.Request = request;
            c.Response = response;

            return c;
        }

    }
}
