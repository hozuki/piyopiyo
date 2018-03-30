using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Extensions;
using OpenMLTD.Piyopiyo.Net.JsonRpc;

namespace OpenMLTD.Piyopiyo.Net.Contributed {
    public class SimulatorServer : JsonRpcServer {

        public SimulatorServer() {
            ScanSessionHandlers(this);
        }

        [MethodHandler(CommonProtocolMethodNames.General_SimInitialize)]
        protected virtual void OnGeneralSimInitialize([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.General_EdExited)]
        protected virtual void OnGeneralEdExited([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_Play)]
        protected virtual void OnPreviewPlay([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_Pause)]
        protected virtual void OnPreviewPause([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_Stop)]
        protected virtual void OnPreviewStop([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_GetPlaybackState)]
        protected virtual void OnPreviewGetPlaybackState([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_SeekByTime)]
        protected virtual void OnPreviewSeekByTime([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Edit_Reload)]
        protected virtual void OnEditReload([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

    }
}
