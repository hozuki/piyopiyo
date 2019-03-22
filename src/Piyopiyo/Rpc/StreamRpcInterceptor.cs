using System;
using System.Diagnostics;
using System.IO;
using Castle.DynamicProxy;
using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo.Rpc {
    internal class StreamRpcInterceptor : RpcInterceptorBase {

        [NotNull]
        private readonly Stream _inputStream;

        [NotNull]
        private readonly Stream _outputStream;

        public StreamRpcInterceptor([NotNull] Stream stream) {
            _outputStream = stream;
            _inputStream = stream;
        }

        public StreamRpcInterceptor([NotNull] Stream sendingStream, [NotNull] Stream receivingStream) {
            _outputStream = sendingStream;
            _inputStream = receivingStream;
        }

        public override void Intercept(IInvocation invocation) {
            try {
                PackInvocationCall(invocation, _outputStream);

                var isNotification = IsInvocationNotification(invocation);

                if (!isNotification) {
                    UnpackInvocationResult(invocation, _inputStream);
                }
            } catch (Exception ex) {
                Debug.Write(ex.ToString());
                throw;
            }
        }

    }
}
