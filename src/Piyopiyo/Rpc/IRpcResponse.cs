using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Entities;

namespace OpenMLTD.Piyopiyo.Rpc {
    public interface IRpcResponse {

        int StatusCode { get; set; }

        void SetBody([NotNull] JsonRpcResponseBase response);

    }
}
