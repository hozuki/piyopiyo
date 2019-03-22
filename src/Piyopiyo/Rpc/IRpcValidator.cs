using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo.Rpc {
    public interface IRpcValidator {

        void Validate([NotNull] IRpcSessionContext context);

    }
}
