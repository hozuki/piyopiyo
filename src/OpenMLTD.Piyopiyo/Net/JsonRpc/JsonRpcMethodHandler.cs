using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {

    public delegate void JsonRpcMethodHandler([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e);

}
