using System.Collections.Generic;
using System.Net.Http;
using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Entities;

namespace OpenMLTD.Piyopiyo.Rpc {
    public interface IRpcRequest {

        [NotNull]
        HttpMethod Method { get; }

        [NotNull]
        IReadOnlyDictionary<string, string> Headers { get; }

        [NotNull]
        JsonRpcRequestBase GetRequestBody();

    }
}
