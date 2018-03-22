using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenMLTD.Piyopiyo.Net;

namespace EditorClientTest {
    internal static class Program {

        private static void Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("Usage: ClientTest.exe <port>");
                return;
            }

            Test(Convert.ToInt32(args[1]));

#if DEBUG
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey(true);
#endif
        }

        private static async void Test(int port) {
            var serverUri = new Uri("http://localhost:" + port.ToString());

            using (var client = new JsonRpcClient(serverUri)) {
                var result = await client.CallAsync(CommonProtocolMethodNames.Preview_Play);

                Console.WriteLine("Response status code: {0} ({1})", (int)result.StatusCode, result.StatusCode);

                if (result.ResponseObject != null) {
                    if (JsonRpcHelper.IsResponseValid(result.ResponseObject)) {
                        var successful = JsonRpcHelper.IsResponseSuccessful(result.ResponseObject);

                        if (successful) {
                            var o = JsonRpcHelper.TranslateAsResponse<PlayRequestResult>(result.ResponseObject);

                            Debug.Assert(o.Result != null, "o.Result != null");

                            Console.WriteLine("Playback starts from: {0}", o.Result.StartFrom);
                        } else {
                            var err = JsonRpcHelper.TranslateAsError(result.ResponseObject);

                            Console.WriteLine("An error occurred: {0}: {1}", err.Error.Code, err.Error.Message);
                        }
                    } else {
                        Console.WriteLine("Response is not valid.");
                    }
                } else {
                    Console.WriteLine("The result is null...");
                }
            }
        }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        private sealed class PlayRequestResult {

            [JsonProperty]
            public TimeSpan StartFrom { get; set; }

        }

    }
}
