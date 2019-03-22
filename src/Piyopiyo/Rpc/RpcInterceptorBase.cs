using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Castle.DynamicProxy;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using OpenMLTD.Piyopiyo.Attributes;
using OpenMLTD.Piyopiyo.Entities;

namespace OpenMLTD.Piyopiyo.Rpc {
    internal abstract class RpcInterceptorBase : IInterceptor {

        public abstract void Intercept([NotNull] IInvocation invocation);

        protected static void PackInvocationCall([NotNull] IInvocation invocation, [NotNull] Stream toStream) {
            var methodName = GetMethodNameFromInvocation(invocation);
            JsonRpcRequestBase payload;

            if (IsInvocationNotification(invocation)) {
                payload = new JsonRpcNotification(methodName);
            } else {
                payload = new JsonRpcRequest(methodName);
            }

            var args = invocation.Arguments;

            if (args.Length > 0) {
                payload.Params = new JArray(args);
            }

            using (var textWriter = new StreamWriter(toStream, Utilities.Utf8, Utilities.DefaultBufferSize, true)) {
                Utilities.Serializer.Serialize(textWriter, payload);
            }
        }

        protected static void UnpackInvocationResult([NotNull] IInvocation invocation, [NotNull] Stream fromStream) {
            if (IsInvocationNotification(invocation)) {
                throw new InvalidOperationException("Cannot read result of notification.");
            }

            var streamBytes = fromStream.ReadAllBytes();
            var o = Utilities.Serializer.Deserialize<JObject>(streamBytes);

            Trace.Assert(o != null, nameof(o) + " != null");

            ValidateResponseFormat(o);
            ValidateResponseContent(o);

            if (o.ContainsKey("error")) {
                var errorResponse = Utilities.Serializer.Deserialize<JsonRpcError>(streamBytes);

                Trace.Assert(errorResponse != null, nameof(errorResponse) + " != null");

                JsonRpcException.Throw(errorResponse.Error.Code, errorResponse.Error.Message);
            }

            var response = Utilities.Serializer.Deserialize<JsonRpcResponse>(streamBytes);

            object resultObject;

            Trace.Assert(response != null, nameof(response) + " != null");

            if (response.Result.IsNull()) {
                resultObject = null;
            } else {
                Debug.Assert(response.Result != null, "response.Result != null");

                resultObject = response.Result.ToObject(invocation.Method.ReturnType, Utilities.Serializer);
            }

            invocation.ReturnValue = resultObject;
        }

        protected static bool IsInvocationNotification([NotNull] IInvocation invocation) {
            return invocation.Method.ReturnType == typeof(void);
        }

        private static void ValidateResponseFormat([NotNull] JObject response) {
            var result = response["result"];
            var error = response["error"];

            if (!((result == null) ^ (error == null))) {
                throw new ArgumentException("'result' and 'error' cannot be both left out or assigned.");
            }

            if (error != null) {
                // Check property missing etc.
                var _ = error.ToObject<JsonRpcErrorInfo>(Utilities.Serializer);
            }
        }

        private static void ValidateResponseContent([NotNull] JObject response) {
            // TODO: validate ID, result type, etc.
        }

        [NotNull]
        private static string GetMethodNameFromInvocation([NotNull] IInvocation invocation) {
            var rewrite = invocation.Method.GetCustomAttribute<RewriteMethodNameAttribute>();

            return rewrite != null ? rewrite.NewName : invocation.Method.Name;
        }

    }
}
