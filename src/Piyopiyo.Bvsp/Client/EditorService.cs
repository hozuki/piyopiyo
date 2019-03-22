using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Bvsp.Entities;

namespace OpenMLTD.Piyopiyo.Bvsp.Client {
    public sealed class EditorService : BvspServiceBase, IBvspEditorServiceProvider {

        public EditorService([NotNull] Uri serviceUri)
            : base(serviceUri) {
        }

        public void NotifySimulatorLaunched(SimLaunchedParam param) {
            var proxy = CreateProxy();

            proxy.NotifySimulatorLaunched(param);
        }

        public void NotifySimulatorExited() {
            var proxy = CreateProxy();

            proxy.NotifySimulatorExited();
        }

        public void NotifyPlaying() {
            var proxy = CreateProxy();

            proxy.NotifyPlaying();
        }

        public void NotifyTick() {
            var proxy = CreateProxy();

            proxy.NotifyTick();
        }

        public void NotifyPaused() {
            var proxy = CreateProxy();

            proxy.NotifyPaused();
        }

        public void NotifyStopped() {
            var proxy = CreateProxy();

            proxy.NotifyStopped();
        }

        public void NotifySought() {
            var proxy = CreateProxy();

            proxy.NotifySought();
        }

        public void NotifyReloaded() {
            var proxy = CreateProxy();

            proxy.NotifyReloaded();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NotNull]
        private IBvspEditorServiceProvider CreateProxy() {
            return CreateProxy<IBvspEditorServiceProvider>();
        }

    }
}
