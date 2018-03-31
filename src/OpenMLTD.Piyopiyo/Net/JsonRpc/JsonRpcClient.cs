using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace OpenMLTD.Piyopiyo.Net.JsonRpc {
    public class JsonRpcClient : DisposableBase {

        public JsonRpcClient() {
            var clientHandler = new HttpClientHandler();

            BaseClientHandler = clientHandler;

            _baseClient = new HttpClient(clientHandler, true);
        }

        [NotNull, ItemNotNull]
        public Task<CallResult> SendRequestAsync([NotNull] Uri serverUri, [NotNull] string method, [CanBeNull] object arguments, [CanBeNull] string id) {
            var requestObject = RequestMessage.FromParamObject(method, arguments, id);

            return CallAsync(serverUri, requestObject);
        }

        [NotNull, ItemNotNull]
        public Task<CallResult> SendRequestAsync([NotNull] Uri serverUri, [NotNull] string method, [CanBeNull, ItemCanBeNull] IEnumerable arguments, [CanBeNull] string id) {
            var requestObject = RequestMessage.FromParams(method, arguments, id);

            return CallAsync(serverUri, requestObject);
        }

        [NotNull, ItemNotNull]
        public Task<CallResult> SendRequestAsync([NotNull] Uri serverUri, [NotNull] string method, [CanBeNull] object arguments, int id) {
            var requestObject = RequestMessage.FromParamObject(method, arguments, id);

            return CallAsync(serverUri, requestObject);
        }

        [NotNull, ItemNotNull]
        public Task<CallResult> SendRequestAsync([NotNull] Uri serverUri, [NotNull] string method, [CanBeNull, ItemCanBeNull] IEnumerable arguments, int id) {
            var requestObject = RequestMessage.FromParams(method, arguments, id);

            return CallAsync(serverUri, requestObject);
        }

        [NotNull, ItemNotNull]
        public Task<CallResult> SendNotificationAsync([NotNull] Uri serverUri, [NotNull] string method, [CanBeNull] object arguments) {
            var requestObject = RequestMessage.FromParamObject(method, arguments);

            return CallAsync(serverUri, requestObject);
        }

        [NotNull, ItemNotNull]
        public Task<CallResult> SendNotificationAsync([NotNull] Uri serverUri, [NotNull] string method, [CanBeNull, ItemCanBeNull] IEnumerable arguments = null) {
            var requestObject = RequestMessage.FromParams(method, arguments);

            return CallAsync(serverUri, requestObject);
        }

        [NotNull]
        public HttpClientHandler BaseClientHandler { get; }

        [NotNull]
        public HttpClient BaseClient => _baseClient;

        protected override void Dispose(bool disposing) {
            _baseClient.Dispose();
        }

        [NotNull, ItemCanBeNull]
        private async Task<CallResult> CallAsync([NotNull] Uri serverUri, [NotNull] RequestMessage requestObject) {
            var requestText = BvspHelper.JsonSerializeRequestToString(requestObject);

            var httpContent = new StringContent(requestText, BvspHelper.Utf8WithoutBom);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue(BvspHelper.BvspContentType) {
                CharSet = BvspHelper.BvspCharSet
            };

            HttpResponseMessage response;

            try {
                response = await _baseClient.PostAsync(serverUri, httpContent);
            } catch (AggregateException ex) {
                Debug.Print(ex.ToString());
                return new CallResult(HttpStatusCode.BadRequest, null);
            } catch (Exception ex) {
                Debug.Print(ex.ToString());
                return new CallResult(HttpStatusCode.BadRequest, null);
            }

            JToken token = null;

            if (response.Content != null) {
                var responseBodyBytes = await response.Content.ReadAsByteArrayAsync();

                if (responseBodyBytes.Length > 0) {
                    token = BvspHelper.JsonDeserialize(responseBodyBytes);
                }
            }

            return new CallResult(response.StatusCode, token);
        }

        private readonly HttpClient _baseClient;

    }
}
