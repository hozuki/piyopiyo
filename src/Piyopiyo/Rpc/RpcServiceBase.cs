using System;
using System.Diagnostics;
using System.Net;
using Castle.DynamicProxy;
using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo.Rpc {
    public abstract class RpcServiceBase {

        [NotNull]
        private readonly ProxyGenerator _proxyGenerator;

        protected RpcServiceBase([NotNull] Uri serviceUri) {
            ServiceUri = serviceUri;
            _proxyGenerator = new ProxyGenerator();
        }

        [NotNull]
        public Uri ServiceUri {
            [DebuggerStepThrough]
            get;
        }

        [NotNull]
        protected T CreateProxy<T>([NotNull] Func<HttpWebRequest> requestFactory)
            where T : class {
            var interceptor = new HttpRpcClientInterceptor(requestFactory);
            var proxy = _proxyGenerator.CreateInterfaceProxyWithoutTarget<T>(interceptor);

            return proxy;
        }

    }
}
