using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Bvsp.Entities;

namespace OpenMLTD.Piyopiyo.Bvsp.Client {
    public sealed class SimulatorService : BvspServiceBase, IBvspSimulatorServiceProvider {

        public SimulatorService([NotNull] Uri serviceUri)
            : base(serviceUri) {
        }

        public SimInitializeResult Initialize(SimInitializeParam param) {
            var proxy = CreateProxy();

            return proxy.Initialize(param);
        }

        public void NotifyEditorExited() {
            var proxy = CreateProxy();

            proxy.NotifyEditorExited();
        }

        public object Play() {
            var proxy = CreateProxy();

            return proxy.Play();
        }

        public object Pause() {
            var proxy = CreateProxy();

            return proxy.Pause();
        }

        public object Stop() {
            var proxy = CreateProxy();

            return proxy.Stop();
        }

        public int GetPlaybackState() {
            var proxy = CreateProxy();

            return proxy.GetPlaybackState();
        }

        public int SeekByTime() {
            var proxy = CreateProxy();

            return proxy.SeekByTime();
        }

        public object Reload(ReloadParam param) {
            var proxy = CreateProxy();

            return proxy.Reload(param);
        }

        public int TestAdd(int a, int b) {
            var proxy = CreateProxy();

            return proxy.TestAdd(a, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NotNull]
        private IBvspSimulatorServiceProvider CreateProxy() {
            return CreateProxy<IBvspSimulatorServiceProvider>();
        }

    }
}
