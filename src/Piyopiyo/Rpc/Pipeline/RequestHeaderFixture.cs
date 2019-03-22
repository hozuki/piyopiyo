using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HTTPnet.Core.Pipeline;

namespace OpenMLTD.Piyopiyo.Rpc.Pipeline {
    // Temporary fix for https://github.com/chkr1011/HTTPnet/issues/4
    internal class RequestHeaderFixture : IHttpContextPipelineHandler {

        public Task ProcessRequestAsync(HttpContextPipelineHandlerContext context) {
            var originalHeaders = context.HttpContext.Request.Headers;
            var newHeaders = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var kv in originalHeaders) {
                newHeaders.Add(kv.Key, kv.Value);
            }

            context.HttpContext.Request.Headers = newHeaders;

            return Task.FromResult(0);
        }

        public Task ProcessResponseAsync(HttpContextPipelineHandlerContext context) {
            return Task.FromResult(0);
        }

    }
}
