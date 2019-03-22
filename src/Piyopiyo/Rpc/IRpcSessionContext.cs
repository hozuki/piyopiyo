using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo.Rpc {
    public interface IRpcSessionContext {

        [NotNull]
        IRpcRequest Request { get; }

        [NotNull]
        IRpcResponse Response { get; }

    }
}
