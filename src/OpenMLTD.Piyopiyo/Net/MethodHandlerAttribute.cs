using System;
using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo.Net {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class MethodHandlerAttribute : Attribute {

        public MethodHandlerAttribute([NotNull] string method) {
            if (string.IsNullOrWhiteSpace(method)) {
                throw new ArgumentException("'" + nameof(method) + "' cannot be null, empty, or contains only whitespace.", nameof(method));
            }

            Method = method;
        }

        [NotNull]
        public string Method { get; }

        public override string ToString() {
            return $"MethodHandler \"{Method}\"";
        }

    }
}
