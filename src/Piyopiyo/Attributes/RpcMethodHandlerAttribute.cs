using System;
using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo.Attributes {
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RpcMethodHandlerAttribute : Attribute {

        public RpcMethodHandlerAttribute([NotNull] string methodName) {
            MethodName = methodName;
        }

        [NotNull]
        public string MethodName { get; }

    }
}
