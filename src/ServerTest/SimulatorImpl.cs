using System;
using OpenMLTD.Piyopiyo.Bvsp;
using OpenMLTD.Piyopiyo.Bvsp.Entities;
using OpenMLTD.Piyopiyo.Bvsp.Entities.Contributed;

namespace ServerTest {
    internal class SimulatorImpl : IBvspSimulatorServiceProvider {

        public SimInitializeResult Initialize(SimInitializeParam param) {
            Console.WriteLine("Initializing...");

            var result = new SimInitializeResult();

            if (param.SupportedFormats != null) {
                if (param.SupportedFormats.Length > 0) {
                    var f = param.SupportedFormats[0];

                    result.SelectedFormat = new SelectedFormatDescriptor {
                        GameId = f.GameId,
                        FormatId = f.FormatId,
                        Version = f.Versions[0],
                        Extra = f.Extra
                    };
                }
            }

            return result;
        }

        public void NotifyEditorExited() {
            Console.WriteLine("Notifying exit...");
        }

        public object Play() {
            Console.WriteLine("Playing...");

            return null;
        }

        public object Pause() {
            Console.WriteLine("Pausing...");

            return null;
        }

        public object Stop() {
            Console.WriteLine("Stopping...");

            return null;
        }

        public int GetPlaybackState() {
            Console.WriteLine("Retrieving playback state...");

            return 0;
        }

        public int SeekByTime() {
            Console.WriteLine("Seek by time...");

            return 0;
        }

        public object Reload(ReloadParam param) {
            Console.WriteLine("Reloading {0}...", param.BeatmapFile);

            return null;
        }

        public int TestAdd(int a, int b) {
            return a + b;
        }

    }
}
