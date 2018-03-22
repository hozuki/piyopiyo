using System.Net;
using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Extensions;
using OpenMLTD.Piyopiyo.Net;

namespace OpenMLTD.Piyopiyo.Editor {
    public class EditorServer : JsonRpcServer {

        public EditorServer() {
            ScanSessionHandlers(this);
        }

        [MethodHandler(CommonProtocolPaths.General_SimInitialized)]
        protected virtual void OnGeneralSimInitialized([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.General_SimExited)]
        protected virtual void OnGeneralSimExited([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.Preview_Playing)]
        protected virtual void OnPreviewPlaying([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.Preview_Tick)]
        protected virtual void OnPreviewTick([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.Preview_Paused)]
        protected virtual void OnPreviewPaused([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.Preview_Stopped)]
        protected virtual void OnPreviewStopped([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

        [MethodHandler(CommonProtocolPaths.Edit_Reloaded)]
        protected virtual void OnEditReloaded([NotNull] object sender, [NotNull] JsonRpcMethodEventArgs e) {
            e.Context.Respond(HttpStatusCode.NotImplemented, JsonRpcServerHelper.EmptyBytes);
        }

    }
}
