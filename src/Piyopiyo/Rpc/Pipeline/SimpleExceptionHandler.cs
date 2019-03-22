using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using HTTPnet.Core.Http;
using HTTPnet.Core.Pipeline;
using OpenMLTD.Piyopiyo.Entities;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace OpenMLTD.Piyopiyo.Rpc.Pipeline {
    internal sealed class SimpleExceptionHandler : IHttpContextPipelineExceptionHandler {

        public Task HandleExceptionAsync(HttpContext httpContext, Exception exception) {
            var response = httpContext.Response;

            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var err = JsonRpcError.CreateEmpty();

            err.Error.Code = exception.HResult;
            err.Error.Message = exception.ToString();

            var memoryStream = new MemoryStream();

            using (var textWriter = new StreamWriter(memoryStream, Utilities.Utf8, Utilities.DefaultBufferSize, true)) {
                Utilities.Serializer.Serialize(textWriter, err);
            }

            memoryStream.Position = 0;

            response.Body = memoryStream;
            response.Headers[HttpHeader.ContentLength] = memoryStream.Length.ToString(CultureInfo.InvariantCulture);

            httpContext.CloseConnection = true;

            return Task.FromResult(0);
        }

    }
}
