using System.Net.Http;
using OpenMLTD.Piyopiyo.Rpc;

namespace OpenMLTD.Piyopiyo.Bvsp.Server {
    internal static class ExtraValidators {

        public sealed class MethodValidator : IRpcValidator {

            public void Validate(IRpcSessionContext context) {
                if (context.Request.Method != HttpMethod.Post) {
                    throw new InvalidRpcRequestException("Only 'POST' method is accepted.");
                }
            }

        }

        public sealed class HeaderValidator : IRpcValidator {

            public void Validate(IRpcSessionContext context) {
                var headers = context.Request.Headers;

                if (!headers.ContainsKey("Content-Length")) {
                    throw new InvalidRpcRequestException("'Content-Length' header is missing.");
                }

                if (!int.TryParse(headers["Content-Length"], out var contentLength) || contentLength <= 0) {
                    throw new InvalidRpcRequestException("'Content-Length' header is incorrect.");
                }

                if (!headers.ContainsKey("Content-Type")) {
                    throw new InvalidRpcRequestException("'Content-Type' header is missing.");
                }

                var contentType = headers["Content-Type"];

                if (contentType != ProtocolConstants.ContentType) {
                    throw new InvalidRpcRequestException("'Content-Type' header is incorrect.");
                }
            }

        }

    }
}
