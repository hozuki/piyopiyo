using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Attributes;
using OpenMLTD.Piyopiyo.Bvsp.Entities;

namespace OpenMLTD.Piyopiyo.Bvsp {
    public interface IBvspSimulatorServiceProvider {

        [RewriteMethodName(CommonProtocolMethodNames.GeneralSimInitialize)]
        SimInitializeResult Initialize([NotNull] SimInitializeParam param);

        [RewriteMethodName(CommonProtocolMethodNames.GeneralEdExited)]
        void NotifyEditorExited();

        [RewriteMethodName(CommonProtocolMethodNames.PreviewPlay)]
        object Play();

        [RewriteMethodName(CommonProtocolMethodNames.PreviewPause)]
        object Pause();

        [RewriteMethodName(CommonProtocolMethodNames.PreviewStop)]
        object Stop();

        [RewriteMethodName(CommonProtocolMethodNames.PreviewGetPlaybackState)]
        int GetPlaybackState();

        [RewriteMethodName(CommonProtocolMethodNames.PreviewSeekByTime)]
        int SeekByTime();

        [RewriteMethodName(CommonProtocolMethodNames.EditReload)]
        object Reload([NotNull] ReloadParam param);

        [RewriteMethodName(CommonProtocolMethodNames.TestAdd)]
        int TestAdd(int a, int b);

    }
}
