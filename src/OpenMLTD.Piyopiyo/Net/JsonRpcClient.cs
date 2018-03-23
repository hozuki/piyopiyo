using System;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace OpenMLTD.Piyopiyo.Net {
    public class JsonRpcClient : DisposableBase {

        public JsonRpcClient() {
            var clientHandler = new HttpClientHandler();

            BaseClientHandler = clientHandler;

            _baseClient = new HttpClient(clientHandler, true);
        }

        [NotNull, ItemNotNull]
        public Task<JsonRpcCallResult> CallAsync([NotNull] Uri serverUri, [NotNull] string method, [CanBeNull] object arguments, [CanBeNull] string id = null) {
            var requestObject = JsonRpcRequestWrapper.FromParamObject(method, arguments, id);

            return CallAsync(serverUri, requestObject);
        }

        [NotNull, ItemNotNull]
        public Task<JsonRpcCallResult> CallAsync([NotNull] Uri serverUri, [NotNull] string method, [CanBeNull, ItemCanBeNull] IEnumerable arguments = null, [CanBeNull] string id = null) {
            var requestObject = JsonRpcRequestWrapper.FromParams(method, arguments, id);

            return CallAsync(serverUri, requestObject);
        }

        [NotNull]
        public HttpClientHandler BaseClientHandler { get; }

        [NotNull]
        public HttpClient BaseClient => _baseClient;

        protected override void Dispose(bool disposing) {
            _baseClient.Dispose();
        }

        private async Task<JsonRpcCallResult> CallAsync([NotNull] Uri serverUri, [NotNull] JsonRpcRequestWrapper requestObject) {
            var requestText = BvspHelper.JsonSerializeToString(requestObject);

            var httpContent = new StringContent(requestText, BvspHelper.Utf8WithoutBom);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue(BvspHelper.BvspContentType) {
                CharSet = BvspHelper.BvspCharSet
            };

            var response = await _baseClient.PostAsync(serverUri, httpContent);

            JToken token = null;

            if (response.Content != null) {
                var responseBodyBytes = await response.Content.ReadAsByteArrayAsync();

                if (responseBodyBytes.Length > 0) {
                    token = BvspHelper.JsonDeserialize(responseBodyBytes);
                }
            }

            return new JsonRpcCallResult(response.StatusCode, token);
        }

        private readonly HttpClient _baseClient;

    }
}
