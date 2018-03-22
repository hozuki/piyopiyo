using System;
using System.Runtime.CompilerServices;

namespace OpenMLTD.Piyopiyo {
    public abstract class DisposableBase : IDisposable {

        ~DisposableBase() {
            if (!IsDisposed) {
                Dispose(false);

                Disposed?.Invoke(this, EventArgs.Empty);
            }

            IsDisposed = true;
        }

        public event EventHandler<EventArgs> Disposed;

        public bool IsDisposed { get; private set; }

        public void Dispose() {
            if (IsDisposed) {
                return;
            }

            Dispose(true);

            Disposed?.Invoke(this, EventArgs.Empty);

            if (!KeepFinalizer) {
                GC.SuppressFinalize(this);
            }

            IsDisposed = true;
        }

        protected bool KeepFinalizer { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void EnsureNotDisposed() {
            if (IsDisposed) {
                throw new ObjectDisposedException("this");
            }
        }

        protected abstract void Dispose(bool disposing);

    }
}
