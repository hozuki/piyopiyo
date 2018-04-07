using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Serialization;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    public abstract class PiyopiyoContractResolver : DefaultContractResolver {

        protected static NamingStrategy Naming {
            get {
                if (_namingStrategy == null) {
                    _namingStrategy = new SnakeCaseNamingStrategy();
                }

                return _namingStrategy;
            }
        }

        [CanBeNull]
        [ThreadStatic]
        private static NamingStrategy _namingStrategy;

    }
}
