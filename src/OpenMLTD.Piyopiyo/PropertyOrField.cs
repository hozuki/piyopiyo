using System;
using System.Reflection;
using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo {
    internal struct PropertyOrField {

        internal PropertyOrField([NotNull] PropertyInfo property) {
            _property = property;
            _field = null;
        }

        internal PropertyOrField([NotNull] FieldInfo field) {
            _property = null;
            _field = field;
        }

        [CanBeNull]
        internal T GetCustomAttribute<T>()
            where T : Attribute {
            if (_property != null) {
                return _property.GetCustomAttribute<T>();
            } else if (_field != null) {
                return _field.GetCustomAttribute<T>();
            } else {
                return null;
            }
        }

        internal void SetValue([NotNull] object obj, [CanBeNull] object value) {
            if (_property != null) {
                _property.SetValue(obj, value);
            } else if (_field != null) {
                _field.SetValue(obj, value);
            }
        }

        [CanBeNull]
        internal Type GetUnderlyingType() {
            if (_property != null) {
                return _property.PropertyType;
            } else if (_field != null) {
                return _field.FieldType;
            } else {
                return null;
            }
        }

        [CanBeNull]
        private readonly PropertyInfo _property;
        [CanBeNull]
        private readonly FieldInfo _field;

    }
}
