using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo.Bvsp.Server {
    public sealed class EditorServer : BvspServerBase {

        public EditorServer(int port, [NotNull] IBvspEditorServiceProvider serviceProvider)
            : base(port) {
            Router.DiscoverRoutesOn(this);

            ServiceProvider = serviceProvider;
        }

        [NotNull]
        [PublicAPI]
        public IBvspEditorServiceProvider ServiceProvider { get; set; }

    }
}
