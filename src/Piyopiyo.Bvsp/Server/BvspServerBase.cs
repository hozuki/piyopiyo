using OpenMLTD.Piyopiyo.Rpc;

namespace OpenMLTD.Piyopiyo.Bvsp.Server {
    public class BvspServerBase : RpcServerBase {

        public BvspServerBase(int port)
            : base(port) {
        }

        protected override IRpcValidator[] GetExtraValidators() {
            return new IRpcValidator[] {
                new ExtraValidators.MethodValidator(),
                new ExtraValidators.HeaderValidator()
            };
        }

    }
}
