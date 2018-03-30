using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using JetBrains.Annotations;
using NHttp;
using OpenMLTD.Piyopiyo.Net.JsonRpc;

namespace OpenMLTD.Piyopiyo.Extensions {
    public static class HttpContextExtensions {

        public static void RpcOk<T>([NotNull] this HttpContext context, [CanBeNull] T result, [CanBeNull] string id = null) {
            var responseObject = ResponseMessage.FromResult(result, id);
            var responseText = BvspHelper.JsonSerializeResponseToString(responseObject);

            Ok(context, responseText);
        }

        public static void RpcOk([NotNull] this HttpContext context, [CanBeNull] string id = null) {
            RpcOk(context, (object)null, id);
        }

        public static void RpcError<T>([NotNull] this HttpContext context, int errorCode, [NotNull] string message, [CanBeNull] T data, [CanBeNull] string id = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest) {
            var responseObject = ResponseMessage.FromError(errorCode, message, data, id);
            var responseText = BvspHelper.JsonSerializeResponseToString(responseObject);

            Respond(context, statusCode, responseText);
        }

        public static void RpcErrorNotImplemented([NotNull] this HttpContext context, [CanBeNull] string id = null) {
            RpcError(context, JsonRpcErrorCodes.InternalError, "Method not implemented", (object)null, id, HttpStatusCode.NotImplemented);
        }

        public static void RpcError([NotNull] this HttpContext context, int errorCode, [NotNull] string message, [CanBeNull] string id = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest) {
            RpcError(context, errorCode, message, (object)null, id, statusCode);
        }

        public static void Ok([NotNull] this HttpContext context, [CanBeNull] string body) {
            Respond(context, HttpStatusCode.OK, body, BvspHelper.Utf8WithoutBom);
        }

        public static void Ok([NotNull] this HttpContext context, [CanBeNull] string body, [NotNull] Encoding bodyEncoding) {
            Respond(context, HttpStatusCode.OK, body, bodyEncoding, null);
        }

        public static void Ok([NotNull] this HttpContext context, [CanBeNull] string body, [NotNull] Encoding bodyEncoding, [CanBeNull] IReadOnlyDictionary<string, string> headers) {
            Respond(context, HttpStatusCode.OK, body, bodyEncoding, headers);
        }

        public static void Respond([NotNull] this HttpContext context, HttpStatusCode statusCode, [CanBeNull] string body) {
            Respond(context, (int)statusCode, body);
        }

        public static void Respond([NotNull] this HttpContext context, int statusCode, [CanBeNull] string body) {
            Respond(context, statusCode, body, BvspHelper.Utf8WithoutBom);
        }

        public static void Respond([NotNull] this HttpContext context, HttpStatusCode statusCode, [CanBeNull] string body, [NotNull] Encoding bodyEncoding) {
            Respond(context, (int)statusCode, body, bodyEncoding);
        }

        public static void Respond([NotNull] this HttpContext context, int statusCode, [CanBeNull] string body, [NotNull] Encoding bodyEncoding) {
            Respond(context, statusCode, body, bodyEncoding, null);
        }

        public static void Respond([NotNull] this HttpContext context, HttpStatusCode statusCode, [CanBeNull] string body, [NotNull] Encoding bodyEncoding, [CanBeNull] IReadOnlyDictionary<string, string> headers) {
            Respond(context, (int)statusCode, body, bodyEncoding, headers);
        }

        public static void Respond([NotNull] this HttpContext context, int statusCode, [CanBeNull] string body, [NotNull] Encoding bodyEncoding, [CanBeNull] IReadOnlyDictionary<string, string> headers) {
            byte[] bytes;

            if (string.IsNullOrEmpty(body)) {
                bytes = BvspHelper.EmptyBytes;
            } else {
                bytes = bodyEncoding.GetBytes(body);
            }

            Respond(context, statusCode, bytes, headers);
        }

        public static void Ok([NotNull] this HttpContext context, [NotNull] byte[] body) {
            Respond(context, HttpStatusCode.OK, body, null);
        }

        public static void Ok([NotNull] this HttpContext context, [NotNull] byte[] body, [CanBeNull] IReadOnlyDictionary<string, string> headers) {
            Respond(context, HttpStatusCode.OK, body, headers);
        }

        public static void Respond([NotNull] this HttpContext context, HttpStatusCode statusCode, [NotNull] byte[] body) {
            Respond(context, (int)statusCode, body, null);
        }

        public static void Respond([NotNull] this HttpContext context, int statusCode, [NotNull] byte[] body) {
            Respond(context, statusCode, body, null);
        }

        public static void Respond([NotNull] this HttpContext context, HttpStatusCode statusCode, [NotNull] byte[] body, [CanBeNull] IReadOnlyDictionary<string, string> headers) {
            Respond(context, (int)statusCode, body, headers);
        }

        public static void Respond([NotNull] this HttpContext context, int statusCode, [NotNull] byte[] body, [CanBeNull] IReadOnlyDictionary<string, string> headers) {
            var response = context.Response;

            response.StatusCode = statusCode;
            response.StatusDescription = ((HttpStatusCode)statusCode).ToString();

            if (headers != null) {
                foreach (var kv in headers) {
                    response.Headers[kv.Key] = kv.Value;
                }
            }

#if DEBUG
            if (response.Headers["Access-Control-Allow-Origin"] == null) {
                response.Headers["Access-Control-Allow-Origin"] = "*";
            }
#endif

            if (response.OutputStream.CanWrite) {
                if (body.Length > 0) {
                    using (var writer = new BinaryWriter(response.OutputStream)) {
                        writer.Write(body);
                    }
                }
            }
        }

    }
}
