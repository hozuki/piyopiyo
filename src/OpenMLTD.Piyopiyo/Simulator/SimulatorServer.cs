using System.Net;
using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Extensions;
using OpenMLTD.Piyopiyo.Net;

namespace OpenMLTD.Piyopiyo.Simulator {
    public class SimulatorServer : JsonRpcServer {

        public SimulatorServer() {
            ScanSessionHandlers(this);
        }

        [MethodHandler(CommonProtocolPaths.General_SimInitialize)]
        protected virtual void OnGeneralSimInitialize([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.General_SimExit)]
        protected virtual void OnGeneralSimExit([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.General_EdExit)]
        protected virtual void OnGeneralEdExit([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.Preview_Play)]
        protected virtual void OnGeneralPreviewPlay([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.Preview_Pause)]
        protected virtual void OnGeneralPreviewPause([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.Preview_Stop)]
        protected virtual void OnGeneralPreviewStop([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.Preview_GetPlaybackState)]
        protected virtual void OnGeneralPreviewGetPlaybackState([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.Preview_GotoTime)]
        protected virtual void OnGeneralPreviewGotoTime([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.Edit_Reload)]
        protected virtual void OnGeneralEditReload([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

    }
}
