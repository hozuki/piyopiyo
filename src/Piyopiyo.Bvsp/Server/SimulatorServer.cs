using System;
using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Attributes;
using OpenMLTD.Piyopiyo.Entities;
using OpenMLTD.Piyopiyo.Rpc;

namespace OpenMLTD.Piyopiyo.Bvsp.Server {
    public class SimulatorServer : BvspServerBase {

        public SimulatorServer(int port, [NotNull] IBvspSimulatorServiceProvider serviceProvider)
            : base(port) {
            Router.DiscoverRoutesOn(this);

            ServiceProvider = serviceProvider;
        }

        [NotNull]
        [PublicAPI]
        public IBvspSimulatorServiceProvider ServiceProvider { get; set; }

        [RpcMethodHandler(CommonProtocolMethodNames.PreviewPlay)]
        [UsedImplicitly]
        private void HandlePreviewPlay([NotNull] IRpcSessionContext context) {
            var body = (JsonRpcRequest)context.Request.GetRequestBody();

            try {
                Deconstruct(body.Params);

                var result = ServiceProvider.Play();

                ReportResult(context, result, body.Id);
            } catch (Exception ex) {
                ReportError(context, ex, body.Id);
            }
        }

        [RpcMethodHandler(CommonProtocolMethodNames.TestAdd)]
        [UsedImplicitly]
        private void HandleTestAdd([NotNull] IRpcSessionContext context) {
            var body = (JsonRpcRequest)context.Request.GetRequestBody();

            try {
                Deconstruct(body.Params, out int a, out int b);

                var result = ServiceProvider.TestAdd(a, b);

                ReportResult(context, result, body.Id);
            } catch (Exception ex) {
                ReportError(context, ex, body.Id);
            }
        }

    }
}
