using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using HTTPnet.Core.Pipeline;
using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Attributes;
using OpenMLTD.Piyopiyo.Rpc.Implementation;
using RouteHandler = System.Action<OpenMLTD.Piyopiyo.Rpc.IRpcSessionContext>;

namespace OpenMLTD.Piyopiyo.Rpc.Pipeline {
    public sealed class Router : IHttpContextPipelineHandler {

        [NotNull]
        private readonly Dictionary<string, RouteHandler> _routes;

        internal Router() {
            _routes = new Dictionary<string, RouteHandler>();
        }

        Task IHttpContextPipelineHandler.ProcessRequestAsync(HttpContextPipelineHandlerContext context) {
            var c = RpcSessionContext.Wrap(context);
            var skeleton = c.GetRequestBodyAsJsonRpc();

            Trace.Assert(skeleton != null, nameof(skeleton) + " != null");

            var methodName = skeleton.Method;

            if (_routes.TryGetValue(methodName, out var handler)) {
                Debug.Print("Found route: {0}", methodName);

                handler.Invoke(c);

                context.BreakPipeline = true;

                return Task.FromResult(0);
            }

            Debug.Print("Registered route not found for method '{0}'", methodName);

            return Task.FromException(new InvalidRpcRequestException($"Route not found for method '{methodName}'"));
        }

        Task IHttpContextPipelineHandler.ProcessResponseAsync(HttpContextPipelineHandlerContext context) {
            return Task.FromResult(0);
        }

        public void AddRoute([NotNull] string methodName, [NotNull] RouteHandler handler) {
            _routes.Add(methodName, handler);
        }

        public void DiscoverRoutesOn([NotNull] RpcServerBase serverObject) {
            var type = serverObject.GetType();
            const BindingFlags allKinds = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            var methods = type.GetMethods(allKinds);

            foreach (var method in methods) {
                var handlerAttr = method.GetCustomAttribute<RpcMethodHandlerAttribute>();

                if (handlerAttr == null) {
                    continue;
                }

                var thiz = method.IsStatic ? null : serverObject;
                var del = (RouteHandler)Delegate.CreateDelegate(typeof(RouteHandler), thiz, method);

                AddRoute(handlerAttr.MethodName, del);
            }
        }

        [Conditional("DEBUG")]
        internal void PrintRoutes() {
            Console.WriteLine("Routes:");

            foreach (var kv in _routes) {
                Console.WriteLine(kv.Key);
            }
        }

    }
}
