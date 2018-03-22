using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo.Net {

    public delegate void JsonRpcMethodHandler([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e);

}
