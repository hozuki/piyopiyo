using System.Diagnostics.CodeAnalysis;

namespace OpenMLTD.Piyopiyo {
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class CommonProtocolMethodNames {

        public const string General_SimLaunched = "general/simLaunched";
        public const string General_SimInitialize = "general/simInitialize";
        public const string General_SimExited = "general/simExited";
        public const string General_EdExited = "general/edExited";

        public const string Preview_Play = "preview/play";
        public const string Preview_Playing = "preview/playing";
        public const string Preview_Tick = "preview/tick";
        public const string Preview_Pause = "preview/pause";
        public const string Preview_Paused = "preview/paused";
        public const string Preview_Stop = "preview/stop";
        public const string Preview_Stopped = "preview/stopped";
        public const string Preview_GetPlaybackState = "preview/getPlaybackState";
        public const string Preview_SeekByTime = "preview/seekByTime";
        public const string Preview_Sought = "preview/sought";

        public const string Edit_Reload = "edit/reload";
        public const string Edit_Reloaded = "edit/reloaded";

    }
}
