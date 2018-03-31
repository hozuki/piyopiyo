using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenMLTD.Piyopiyo;
using OpenMLTD.Piyopiyo.Net.JsonRpc;

namespace EditorClientTest {
    internal static class Program {

        private static void Main(string[] args) {
            int serverPort;

            if (args.Length < 1) {
                //Console.WriteLine("Usage: ClientTest.exe <port>");
                //return;

                Console.Write("Server port: ");

                var ln = Console.ReadLine();

                serverPort = Convert.ToInt32(ln);
            } else {
                serverPort = Convert.ToInt32(args[1]);
            }

            Task.WaitAll(WorkerLoop(serverPort));

#if DEBUG
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey(true);
#endif
        }

        private static async Task WorkerLoop(int serverPort) {
            Console.Write("Command: ");
            var simpleCommand = Console.ReadLine();

            var b = await Test(serverPort, simpleCommand);

            while (b) {
                Console.Write("Command: ");
                simpleCommand = Console.ReadLine();

                b = await Test(serverPort, simpleCommand);
            }
        }

        private static async Task<bool> Test(int port, string simpleCommand) {
            string methodName;

            switch (simpleCommand.ToLowerInvariant()) {
                case "exit":
                case "quit":
                    return false;
                case "play":
                    methodName = CommonProtocolMethodNames.Preview_Play;
                    break;
                case "pause":
                    methodName = CommonProtocolMethodNames.Preview_Pause;
                    break;
                case "stop":
                    methodName = CommonProtocolMethodNames.Preview_Stop;
                    break;
                default:
                    Console.WriteLine("Unknown simple command: {0}", simpleCommand);
                    return true;
            }

            var serverUri = new Uri("http://localhost:" + port.ToString());

            using (var client = new JsonRpcClient()) {
                var result = await client.SendRequestAsync(serverUri, methodName, null, null);

                Console.WriteLine("Response status code: {0} ({1})", (int)result.StatusCode, result.StatusCode);

                if (result.ResponseObject != null) {
                    Console.WriteLine("Got response or error.");

                    //if (JsonRpcHelper.IsResponseValid(result.ResponseObject)) {
                    //    var successful = JsonRpcHelper.IsResponseSuccessful(result.ResponseObject);

                    //    if (successful) {
                    //        var o = JsonRpcHelper.TranslateAsResponse<PlayRequestResult>(result.ResponseObject);

                    //        Debug.Assert(o.Result != null, "o.Result != null");

                    //        Console.WriteLine("Playback starts from: {0}", o.Result.StartFrom);
                    //    } else {
                    //        var err = JsonRpcHelper.TranslateAsError(result.ResponseObject);

                    //        Console.WriteLine("An error occurred: {0}: {1}", err.Error.Code, err.Error.Message);
                    //    }
                    //} else {
                    //    Console.WriteLine("Response is not valid.");
                    //}
                } else {
                    Console.WriteLine("The result is null...");
                }
            }

            return true;
        }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        private sealed class PlayRequestResult {

            [JsonProperty]
            public TimeSpan StartFrom { get; set; }

        }

    }
}
