using System.Diagnostics;
using System.Threading.Tasks;
using HTTPnet.Core.Pipeline;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using OpenMLTD.Piyopiyo.Entities;
using OpenMLTD.Piyopiyo.Rpc.Implementation;

namespace OpenMLTD.Piyopiyo.Rpc.Pipeline {
    internal sealed class JsonRpcValidator : IHttpContextPipelineHandler {

        [NotNull]
        [ItemNotNull]
        private readonly IRpcValidator[] _validators;

        public JsonRpcValidator([NotNull] [ItemNotNull] params IRpcValidator[] validators) {
            _validators = validators;
        }

        public Task ProcessRequestAsync(HttpContextPipelineHandlerContext context) {
            ValidateRequestFormat(context, out var payload);

            var c = RpcSessionContext.Wrap(context);

            c.SaveRequestBody(payload);

            return Task.FromResult(0);
        }

        public Task ProcessResponseAsync(HttpContextPipelineHandlerContext context) {
            return Task.FromResult(0);
        }

        private void ValidateRequestFormat([NotNull] HttpContextPipelineHandlerContext context, [NotNull] out JsonRpcRequestBase payload) {
            var c = RpcSessionContext.Wrap(context);

            foreach (var validator in _validators) {
                validator.Validate(c);
            }

            var body = context.HttpContext.Request.Body.ReadAllBytes();
            var o = Utilities.Serializer.Deserialize<JObject>(body);

            Trace.Assert(o != null, nameof(o) + " != null");

            // TODO: check object properties
            // e.g.: o.ContainsKey("method") 

            if (o.ContainsKey("id")) {
                payload = o.ToObject<JsonRpcRequest>();
            } else {
                payload = o.ToObject<JsonRpcNotification>();
            }
        }

    }
}
