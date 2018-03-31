using System;
using System.Globalization;
using JetBrains.Annotations;

namespace OpenMLTD.Piyopiyo.Extensions {
    internal static class DateTimeExtensions {

        // Used in server communications.
        // Mon, 01 Jan 9999 00:00:00 GMT
        [NotNull]
        internal static string ToGmtString(this DateTime dateTime) {
            return dateTime.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", UsCulture.Value);
        }

        private static readonly Lazy<CultureInfo> UsCulture = new Lazy<CultureInfo>(() => CultureInfo.GetCultureInfo("en-US"));

    }
}
