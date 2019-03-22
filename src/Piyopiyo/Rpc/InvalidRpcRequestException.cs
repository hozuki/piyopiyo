using System.Net;

namespace OpenMLTD.Piyopiyo.Rpc {
    public class InvalidRpcRequestException : HttpListenerException {

        public InvalidRpcRequestException() {
        }

        public InvalidRpcRequestException(int errorCode)
            : base(errorCode) {
        }

        public InvalidRpcRequestException(string message)
            : base((int)HttpStatusCode.InternalServerError, message) {
        }

        public InvalidRpcRequestException(int errorCode, string message)
            : base(errorCode, message) {
        }

    }
}
