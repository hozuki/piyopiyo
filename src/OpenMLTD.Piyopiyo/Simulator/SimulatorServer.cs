using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Extensions;
using OpenMLTD.Piyopiyo.Net;

namespace OpenMLTD.Piyopiyo.Simulator {
    public class SimulatorServer : JsonRpcServer {

        public SimulatorServer() {
            ScanSessionHandlers(this);
        }

        [MethodHandler(CommonProtocolMethodNames.General_SimInitialize)]
        protected virtual void OnGeneralSimInitialize([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.General_SimExit)]
        protected virtual void OnGeneralSimExit([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.General_EdExit)]
        protected virtual void OnGeneralEdExit([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_Play)]
        protected virtual void OnGeneralPreviewPlay([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_Pause)]
        protected virtual void OnGeneralPreviewPause([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_Stop)]
        protected virtual void OnGeneralPreviewStop([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_GetPlaybackState)]
        protected virtual void OnGeneralPreviewGetPlaybackState([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Preview_GotoTime)]
        protected virtual void OnGeneralPreviewGotoTime([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

        [MethodHandler(CommonProtocolMethodNames.Edit_Reload)]
        protected virtual void OnEditReload([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.RpcErrorNotImplemented();
        }

    }
}
