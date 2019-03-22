using System;
using System.Diagnostics;
using HTTPnet;
using HTTPnet.Core;
using HTTPnet.Core.Http;
using HTTPnet.Core.Pipeline;
using HTTPnet.Core.Pipeline.Handlers;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using OpenMLTD.Piyopiyo.Entities;
using OpenMLTD.Piyopiyo.Rpc.Pipeline;

namespace OpenMLTD.Piyopiyo.Rpc {
    public abstract class RpcServerBase {

        [NotNull]
        private readonly HttpServerFactory _factory;

        private readonly int _port;

        [CanBeNull]
        private HttpServer _server;

        protected RpcServerBase(int port) {
            _factory = new HttpServerFactory();
            _port = port;
            Router = new Router();
        }

        public bool IsRunning {
            [DebuggerStepThrough]
            get;
            [DebuggerStepThrough]
            private set;
        }

        [NotNull]
        protected Router Router {
            [DebuggerStepThrough]
            get;
        }

        public void Start() {
            if (IsRunning) {
                throw new InvalidOperationException();
            }

            var server = _factory.CreateHttpServer();

            Configure(server);

            var startOptions = new HttpServerOptions {
                Port = _port
            };

            server.StartAsync(startOptions).GetAwaiter().GetResult();

            _server = server;

            IsRunning = true;
        }

        public void Stop() {
            if (!IsRunning) {
                throw new InvalidOperationException();
            }

            try {
                _server?.StopAsync().Wait();
                _server?.Dispose();
            } catch (Exception ex) {
                Debug.Print(ex.ToString());
            }

            _server = null;

            IsRunning = false;
        }

        protected static void ReportResult<T>([NotNull] IRpcSessionContext context, [CanBeNull] T result, [CanBeNull] object id = null) {
            var resp = JsonRpcResponse.CreateEmpty();

            if (ReferenceEquals(result, null)) {
                resp.Result = JValue.CreateNull();
            } else {
                resp.Result = JToken.FromObject(result);
            }

            resp.Id = id;

            var response = context.Response;

            response.StatusCode = (int)HttpStatusCode.OK;
            response.SetBody(resp);
        }

        protected static void ReportError([NotNull] IRpcSessionContext context, [NotNull] Exception ex, [CanBeNull] object id = null) {
            var err = JsonRpcError.CreateEmpty();

            err.Id = id;
            err.Error.Code = ex.HResult;
            err.Error.Message = ex.Message;

            var response = context.Response;

            response.StatusCode = (int)HttpStatusCode.OK;
            response.SetBody(err);
        }

        protected static void Deconstruct([CanBeNull] JArray array) {
            Debug.Assert(array != null);

            RequireArrayLength(array, 0);
        }

        protected static void Deconstruct<T>([CanBeNull] JArray array, [CanBeNull] out T v) {
            Debug.Assert(array != null);

            RequireArrayLength(array, 1);

            v = array[0].ToObject<T>(Utilities.Serializer);
        }

        protected static void Deconstruct<T1, T2>([CanBeNull] JArray array, [CanBeNull] out T1 v1, [CanBeNull] out T2 v2) {
            Debug.Assert(array != null);

            RequireArrayLength(array, 2);

            v1 = array[0].ToObject<T1>(Utilities.Serializer);
            v2 = array[1].ToObject<T2>(Utilities.Serializer);
        }

        protected static void Deconstruct<T1, T2, T3>([CanBeNull] JArray array, [CanBeNull] out T1 v1, [CanBeNull] out T2 v2, [CanBeNull] out T3 v3) {
            Debug.Assert(array != null);

            RequireArrayLength(array, 3);

            v1 = array[0].ToObject<T1>(Utilities.Serializer);
            v2 = array[1].ToObject<T2>(Utilities.Serializer);
            v3 = array[2].ToObject<T3>(Utilities.Serializer);
        }

        protected static void Deconstruct<T1, T2, T3, T4>([CanBeNull] JArray array, [CanBeNull] out T1 v1, [CanBeNull] out T2 v2, [CanBeNull] out T3 v3, [CanBeNull] out T4 v4) {
            Debug.Assert(array != null);

            RequireArrayLength(array, 4);

            v1 = array[0].ToObject<T1>(Utilities.Serializer);
            v2 = array[1].ToObject<T2>(Utilities.Serializer);
            v3 = array[2].ToObject<T3>(Utilities.Serializer);
            v4 = array[3].ToObject<T4>(Utilities.Serializer);
        }

        [NotNull]
        [ItemNotNull]
        protected virtual IRpcValidator[] GetExtraValidators() {
            return Array.Empty<IRpcValidator>();
        }

        private void Configure([NotNull] IHttpServer server) {
            var pipeline = new HttpContextPipeline(new SimpleExceptionHandler());

            pipeline.Add(new RequestHeaderFixture());
            pipeline.Add(new RequestBodyHandler());
            pipeline.Add(new ResponseBodyLengthHandler());
            pipeline.Add(new ResponseCompressionHandler());
            pipeline.Add(new JsonRpcValidator(GetExtraValidators()));
            pipeline.Add(Router);

            server.RequestHandler = pipeline;
        }

        private static void RequireArrayLength([NotNull] JArray array, int length) {
            if (array.Count != length) {
                throw new ArgumentException($"Expected array length {array.Count}, actual {length}");
            }
        }

    }
}
