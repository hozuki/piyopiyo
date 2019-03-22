using System;
using System.Diagnostics;
using System.Net;
using Castle.DynamicProxy;
using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo.Rpc {
    internal class HttpRpcClientInterceptor : RpcInterceptorBase {

        [NotNull]
        private readonly Func<HttpWebRequest> _requestFactory;

        public HttpRpcClientInterceptor([NotNull] Func<HttpWebRequest> requestFactory) {
            _requestFactory = requestFactory;
        }

        public override void Intercept(IInvocation invocation) {
            var httpRequest = _requestFactory.Invoke();

            try {
                PackInvocationCall(invocation, httpRequest.GetRequestStream());

                var isNotification = IsInvocationNotification(invocation);

                var httpResponse = httpRequest.GetResponse();

                if (!isNotification) {
                    var responseStream = httpResponse.GetResponseStream();

                    if (responseStream == null) {
                        throw new NullReferenceException();
                    }

                    UnpackInvocationResult(invocation, responseStream);
                }
            } catch (Exception ex) {
                Debug.Write(ex.ToString());
                throw;
            }
        }

    }
}
