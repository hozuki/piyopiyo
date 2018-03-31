using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using NHttp;
using OpenMLTD.Piyopiyo.Extensions;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    public abstract class JsonRpcServer : DisposableBase {

        protected JsonRpcServer() {
            var server = new HttpServer();

            server.RequestReceived += Server_RequestReceived;
            server.UnhandledException += Server_UnhandledException;
            server.ServerBanner = ServerBanner;

            _server = server;
        }

        public void Start(int port) {
            Start(IPAddress.Loopback, port);
        }

        public void Start([NotNull] IPAddress ipAddress, int port) {
            if (IsRunning) {
                return;
            }

            _server.EndPoint = new IPEndPoint(ipAddress, port);
            _server.Start();

            IsRunning = true;
        }

        public void Stop() {
            if (!IsRunning) {
                return;
            }

            _server.Stop();

            IsRunning = false;
        }

        public HttpServerState ServerState => _server.State;

        public IPEndPoint EndPoint => _server.EndPoint;

        public bool IsRunning { get; private set; }

        internal const string ServerBanner = "Piyopiyo";

        protected HttpServer BaseServer => _server;

        protected override void Dispose(bool disposing) {
            _server.RequestReceived -= Server_RequestReceived;
            _server.UnhandledException -= Server_UnhandledException;
            _server.Dispose();
        }

        protected void ScanSessionHandlers([NotNull] object obj) {
            _handlers.Clear();

            const BindingFlags sessionHandlerBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            var methods = obj.GetType().GetMethods(sessionHandlerBindingFlags);

            foreach (var method in methods) {
                var attr = method.GetCustomAttribute<MethodHandlerAttribute>();

                if (attr == null) {
                    continue;
                }

                JsonRpcMethodHandler del;

                if (method.IsStatic) {
                    del = Delegate.CreateDelegate(typeof(JsonRpcMethodHandler), method) as JsonRpcMethodHandler;
                } else {
                    del = Delegate.CreateDelegate(typeof(JsonRpcMethodHandler), obj, method, false) as JsonRpcMethodHandler;
                }

                if (del == null) {
                    continue;
                }

                _handlers[attr] = del;
            }
        }

        [NotNull]
        private static RequestValidationResult ValidateRequest([NotNull] HttpRequest request) {
            var r = new RequestValidationResult {
                IsValid = false
            };

            if (!request.HttpMethod.Equals("post", StringComparison.OrdinalIgnoreCase)) {
                r.ErrorDescription = $"Method \"{request.HttpMethod}\" is not allowed here.";
                r.SuggestedHttpStatusCode = HttpStatusCode.MethodNotAllowed;

                return r;
            }

            var headers = request.Headers;

            var contentLengthStr = headers.Get("Content-Length");

            if (contentLengthStr == null) {
                r.ErrorDescription = "A header is missing: Content-Length.";
                r.SuggestedHttpStatusCode = HttpStatusCode.LengthRequired;

                return r;
            }

            long contentLengthValue;

            try {
                contentLengthValue = Convert.ToInt64(contentLengthStr);
            } catch (Exception ex) {
                r.ErrorDescription = $"An exception occurred while trying to parse content length: {ex}";
                r.SuggestedHttpStatusCode = HttpStatusCode.LengthRequired;

                return r;
            }

            if (contentLengthValue <= 0) {
                r.ErrorDescription = $"Invalid content length: \"{contentLengthStr}\".";
                r.SuggestedHttpStatusCode = HttpStatusCode.LengthRequired;

                return r;
            }

            if (contentLengthValue > MaxRequestBodySize) {
                r.ErrorDescription = $"Content too large: {contentLengthValue} bytes.";
                r.SuggestedHttpStatusCode = HttpStatusCode.RequestEntityTooLarge;

                return r;
            }

            var contentTypeStr = headers.Get("Content-Type");

            if (contentTypeStr == null) {
                r.ErrorDescription = "A header is missing: Content-Type.";
                r.SuggestedHttpStatusCode = HttpStatusCode.NotAcceptable;

                return r;
            }

            MediaTypeHeaderValue mediaTypeHeader;

            try {
                mediaTypeHeader = MediaTypeHeaderValue.Parse(contentTypeStr);
            } catch (Exception ex) {
                r.ErrorDescription = $"Failed to parse content type \"{contentTypeStr}\": {ex}";
                r.SuggestedHttpStatusCode = HttpStatusCode.NotAcceptable;

                return r;
            }

            if (!mediaTypeHeader.MediaType.Equals(BvspHelper.BvspContentType, StringComparison.OrdinalIgnoreCase)
                || !mediaTypeHeader.CharSet.Equals(BvspHelper.BvspCharSet, StringComparison.OrdinalIgnoreCase)) {
                r.ErrorDescription = $"Invalid content type: \"{contentTypeStr}\".";
                r.SuggestedHttpStatusCode = HttpStatusCode.NotAcceptable;

                return r;
            }

            byte[] body;

            try {
                body = request.InputStream.ReadMaxConstrained(contentLengthValue);
            } catch (TimeoutException ex) {
                r.ErrorDescription = $"An exception occurred while trying to read request body: {ex}";
                r.SuggestedHttpStatusCode = HttpStatusCode.RequestTimeout;

                return r;
            } catch (Exception ex) {
                r.ErrorDescription = $"An exception occurred while trying to read request body: {ex}";
                r.SuggestedHttpStatusCode = HttpStatusCode.BadRequest;

                return r;
            }

            string bodyStr;

            try {
                bodyStr = BvspHelper.Utf8WithoutBom.GetString(body);
            } catch (Exception ex) {
                r.ErrorDescription = $"An exception occurred while trying to parse request body as string: {ex}";
                r.SuggestedHttpStatusCode = HttpStatusCode.BadRequest;
                r.SuggestedJsonRpcErrorCode = JsonRpcErrorCodes.ParseError;

                return r;
            }

            JToken token;

            try {
                token = BvspHelper.JsonDeserialize(bodyStr);
            } catch (Exception ex) {
                r.ErrorDescription = $"An exception occurred while trying to parse request body as JSON: {ex}";
                r.SuggestedHttpStatusCode = HttpStatusCode.BadRequest;
                r.SuggestedJsonRpcErrorCode = JsonRpcErrorCodes.ParseError;

                return r;
            }

            if (!(token is JObject jobj)) {
                r.ErrorDescription = "The request body is not a JSON object.";
                r.SuggestedHttpStatusCode = HttpStatusCode.BadRequest;
                r.SuggestedJsonRpcErrorCode = JsonRpcErrorCodes.ParseError;

                return r;
            }

            if (!JsonRpcHelper.IsRequestValid(jobj, out string jsonSchemaMessage)) {
                r.ErrorDescription = "The request object is not a single JSON RPC 2.0 request object:\n" + jsonSchemaMessage;
                r.SuggestedHttpStatusCode = HttpStatusCode.BadRequest;

                return r;
            }

            r.IsValid = true;
            r.ParsedBody = bodyStr;
            r.ParsedObject = jobj;

            return r;
        }

        private void DispatchRequest([NotNull] JsonRpcMethodEventArgs e) {
            JsonRpcMethodHandler handler = null;

            var method = e.ParsedRequestObject["method"]?.Value<string>();

            if (method != null) {
                foreach (var kv in _handlers) {
                    if (kv.Key.Method == method) {
                        handler = kv.Value;
                        break;
                    }
                }
            }

            if (handler != null) {
                handler.Invoke(this, e);
            } else {
                MethodNotFoundRequestHandler(this, e);
            }
        }

        private void Server_RequestReceived(object sender, HttpRequestEventArgs e) {
#if DEBUG
            if (e.Request.HttpMethod.Equals("options", StringComparison.OrdinalIgnoreCase)) {
                // Handles testing in browsers' fetch().
                // Preflight test: https://developer.mozilla.org/zh-CN/docs/Web/HTTP/Access_control_CORS
                var headers = new Dictionary<string, string> {
                    ["Access-Control-Allow-Origin"] = "*",
                    ["Access-Control-Allow-Methods"] = "POST, GET, OPTIONS",
                    ["Access-Control-Allow-Headers"] = "Content-Type, Content-Length"
                };

                e.Context.Respond(HttpStatusCode.OK, BvspHelper.EmptyBytes, headers);

                return;
            }
#endif

            var r = ValidateRequest(e.Request);

            if (r.IsValid) {
                Debug.Assert(r.ParsedBody != null, "r.ParsedBody != null");
                Debug.Assert(r.ParsedObject != null, "r.ParsedObject != null");

                var args = new JsonRpcMethodEventArgs(e.Context, r.ParsedBody, r.ParsedObject);

                DispatchRequest(args);
            } else {
                var httpStatusCode = r.SuggestedHttpStatusCode;

                if (httpStatusCode == 0) {
                    httpStatusCode = HttpStatusCode.BadRequest;
                }

                var rpcErrorCode = r.SuggestedJsonRpcErrorCode;

                if (rpcErrorCode == 0) {
                    rpcErrorCode = JsonRpcErrorCodes.InvalidRequest;
                }

                var errorDescription = r.ErrorDescription;

                if (errorDescription == null) {
                    errorDescription = "Bad Request";
                }

                e.Context.RpcError(rpcErrorCode, errorDescription, statusCode: httpStatusCode);
            }
        }

        private void Server_UnhandledException(object sender, HttpExceptionEventArgs e) {
            var description = e.Exception.ToString();

            Debug.Print(description);

            e.Context.RpcError(JsonRpcErrorCodes.InternalError, description, statusCode: HttpStatusCode.InternalServerError);
        }

        private void MethodNotFoundRequestHandler(object sender, JsonRpcMethodEventArgs e) {
            var requestObject = e.ParsedRequestObject;
            var rpcMethod = requestObject["method"]?.Value<string>();

            if (rpcMethod == null) {
                rpcMethod = "(missing method)";
            }

            e.Context.RpcError(JsonRpcErrorCodes.MethodNotFound, $"Method \"{rpcMethod}\" is not supported on this server.");
        }

        [NotNull]
        private sealed class RequestValidationResult {

            internal bool IsValid { get; set; }

            internal HttpStatusCode SuggestedHttpStatusCode { get; set; }

            internal int SuggestedJsonRpcErrorCode { get; set; }

            [CanBeNull]
            internal string ErrorDescription { get; set; }

            [CanBeNull]
            internal string ParsedBody { get; set; }

            [CanBeNull]
            internal JObject ParsedObject { get; set; }

        }

        private const long MaxRequestBodySize = 10 * 1024 * 1024;

        private readonly HttpServer _server;

        private readonly Dictionary<MethodHandlerAttribute, JsonRpcMethodHandler> _handlers = new Dictionary<MethodHandlerAttribute, JsonRpcMethodHandler>();

    }
}
