using System.Diagnostics;
using System.IO;
using HTTPnet.Core.Http.Raw;
using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Entities;

namespace OpenMLTD.Piyopiyo.Rpc.Implementation {
    internal sealed class RpcSessionResponse : IRpcResponse {

        public RpcSessionResponse([NotNull] RpcSessionContext context) {
            Context = context;
            RawResponse = context.RawContext.HttpContext.Response;
        }

        [NotNull]
        public RpcSessionContext Context { get; }

        [NotNull]
        public RawHttpResponse RawResponse { get; }

        public int StatusCode {
            [DebuggerStepThrough]
            get => RawResponse.StatusCode;
            [DebuggerStepThrough]
            set => RawResponse.StatusCode = value;
        }

        public void SetBody(JsonRpcResponseBase response) {
            var memoryStream = new MemoryStream();

            using (var textWriter = new StreamWriter(memoryStream, Utilities.Utf8, Utilities.DefaultBufferSize, true)) {
                Utilities.Serializer.Serialize(textWriter, response);
            }

            memoryStream.Position = 0;

            RawResponse.Body = memoryStream;
        }

    }
}
