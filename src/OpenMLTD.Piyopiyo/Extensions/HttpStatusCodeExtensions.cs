using System.Net;
using System.Runtime.CompilerServices;

namespace OpenMLTD.Piyopiyo.Extensions {
    public static class HttpStatusCodeExtensions {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsContinual(this HttpStatusCode statusCode) {
            return (int)statusCode >= 100 && (int)statusCode <= 199;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSuccessful(this HttpStatusCode statusCode) {
            return (int)statusCode >= 200 && (int)statusCode <= 299;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsActionRequired(this HttpStatusCode statusCode) {
            return (int)statusCode >= 300 && (int)statusCode <= 399;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsClientError(this HttpStatusCode statusCode) {
            return (int)statusCode >= 400 && (int)statusCode <= 499;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsServerError(this HttpStatusCode statusCode) {
            return (int)statusCode >= 500 && (int)statusCode <= 599;
        }

    }
}
