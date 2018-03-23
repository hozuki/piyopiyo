using System;
using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo {
    internal static class TypeHelper {

        [CanBeNull]
        internal static object Default([NotNull] Type type) {
            if (type.IsValueType) {
                return Activator.CreateInstance(type);
            } else {
                return null;
            }
        }

    }
}
