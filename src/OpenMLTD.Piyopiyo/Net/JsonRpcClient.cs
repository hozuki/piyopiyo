using System;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace OpenMLTD.Piyopiyo.Net {
    public sealed class JsonRpcClient : DisposableBase {

        public JsonRpcClient([NotNull] Uri serverUri) {
            var clientHandler = new HttpClientHandler();

            BaseClientHandler = clientHandler;

            _baseClient = new HttpClient(clientHandler, true);

            ServerUri = serverUri;
        }

        [NotNull, ItemNotNull]
        public async Task<JsonRpcCallResult> CallAsync([NotNull] string method, [CanBeNull, ItemCanBeNull] IEnumerable arguments = null, [CanBeNull] string id = null) {
            var requestObject = JsonRpcRequestWrapper.FromParams(method, arguments, id);
            var requestText = BvspHelper.JsonSerializeToString(requestObject);

            var httpContent = new StringContent(requestText, BvspHelper.Utf8WithoutBom);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue(BvspHelper.BvspContentType) {
                CharSet = BvspHelper.BvspCharSet
            };

            var response = await _baseClient.PostAsync(ServerUri, httpContent);

            JToken token = null;

            if (response.Content != null) {
                var responseBodyBytes = await response.Content.ReadAsByteArrayAsync();

                if (responseBodyBytes.Length > 0) {
                    token = BvspHelper.JsonDeserialize(responseBodyBytes);
                }
            }

            return new JsonRpcCallResult(response.StatusCode, token);
        }

        public HttpClientHandler BaseClientHandler { get; }

        public HttpClient BaseClient => _baseClient;

        public Uri ServerUri { get; }

        protected override void Dispose(bool disposing) {
            _baseClient.Dispose();
        }

        private readonly HttpClient _baseClient;

    }
}
