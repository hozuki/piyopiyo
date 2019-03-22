using System;
using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo.Attributes {
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RewriteMethodNameAttribute : Attribute {

        public RewriteMethodNameAttribute([NotNull] string newName) {
            NewName = newName;
        }

        [NotNull]
        public string NewName { get; }

    }
}
