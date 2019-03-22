using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Attributes;
using OpenMLTD.Piyopiyo.Bvsp.Entities;

namespace OpenMLTD.Piyopiyo.Bvsp {
    public interface IBvspEditorServiceProvider {

        [RewriteMethodName(CommonProtocolMethodNames.GeneralSimLaunched)]
        void NotifySimulatorLaunched([NotNull] SimLaunchedParam param);

        [RewriteMethodName(CommonProtocolMethodNames.GeneralSimExited)]
        void NotifySimulatorExited();

        [RewriteMethodName(CommonProtocolMethodNames.PreviewPlaying)]
        void NotifyPlaying();

        [RewriteMethodName(CommonProtocolMethodNames.PreviewTick)]
        void NotifyTick();

        [RewriteMethodName(CommonProtocolMethodNames.PreviewPaused)]
        void NotifyPaused();

        [RewriteMethodName(CommonProtocolMethodNames.PreviewStopped)]
        void NotifyStopped();

        [RewriteMethodName(CommonProtocolMethodNames.PreviewSought)]
        void NotifySought();

        [RewriteMethodName(CommonProtocolMethodNames.EditReloaded)]
        void NotifyReloaded();

    }
}
