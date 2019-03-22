using System;
using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo.Rpc {
    public class JsonRpcException : ApplicationException {

        public JsonRpcException(int code, [CanBeNull] string rpcMessage) {
            Code = code;
            RpcMessage = rpcMessage;
        }

        private JsonRpcException([NotNull] string message, int code, [CanBeNull] string rpcMessage)
            : base(message) {
            Code = code;
            RpcMessage = rpcMessage;
        }

        public int Code { get; }

        /// <summary>
        ///     Gets the error message returned by the JSON RPC server.
        /// </summary>
        [CanBeNull]
        public string RpcMessage { get; }

        [ContractAnnotation("=> halt")]
        internal static void Throw(int code, [CanBeNull] string rpcMessage) {
            var message = $"An exception was raised in RPC process. Code: {code.ToString()}; Message: {rpcMessage}";

            throw new JsonRpcException(message, code, rpcMessage);
        }

    }
}
