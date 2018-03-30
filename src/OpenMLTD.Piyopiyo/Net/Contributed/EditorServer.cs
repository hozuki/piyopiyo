using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Extensions;
using OpenMLTD.Piyopiyo.Net.JsonRpc;

namespace OpenMLTD.Piyopiyo.Net.Contributed {
    public class EditorServer : JsonRpcServer {

        public EditorServer() {
            ScanSessionHandlers(this);
        }

        [MethodHandler(CommonProtocolMethodNames.General_SimLaunched)]
        protected virtual void OnGeneralSimLaunched([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.General_SimExited)]
        protected virtual void OnGeneralSimExited([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_Playing)]
        protected virtual void OnPreviewPlaying([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_Tick)]
        protected virtual void OnPreviewTick([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_Paused)]
        protected virtual void OnPreviewPaused([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_Stopped)]
        protected virtual void OnPreviewStopped([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_Sought)]
        protected virtual void OnPreviewSought([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Edit_Reloaded)]
        protected virtual void OnEditReloaded([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

    }
}
