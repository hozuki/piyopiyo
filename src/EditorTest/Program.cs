using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenMLTD.Piyopiyo.Editor;
using OpenMLTD.Piyopiyo.Net;

namespace EditorTest {
    internal static class Program {

        private static void Main(string[] args) {
            TestRequestDeserialization();
            TestRequestWrapper();

            var server = new EditorServer();

            var port = 0;

            if (args.Length > 0) {
                port = Convert.ToInt32(args[0]);
            }

            server.Start(port);

            Console.WriteLine("Editor server started on port {0}. Press Enter to exit.", server.EndPoint.Port);

            do {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter) {
                    break;
                }
            } while (true);

            Console.WriteLine("Stopping...");

            server.Stop();
            server.Dispose();

            Console.WriteLine("Stopped.");

#if DEBUG
            Console.WriteLine("You are in debug mode. Press any key again to exit.");
            Console.ReadKey(true);
#endif
        }

        private static void TestRequestWrapper() {
            const string json = @"{
    ""jsonrpc"" : ""2.0"",
    ""method"": ""method1"",
    ""params"" : [
        { ""name"" : ""My Name"", ""age"" : 10000 },
        10,
        ""2012-04-23T18:25:43.511Z""
    ],
    ""id"": null
}";

            var jobj = JObject.Parse(json);
            var obj = JsonRpcHelper.TranslateAsRequest(jobj);
            var @params = JsonRpcRequestWrapper.ParamArrayToObject<SimpleClass>(obj.Params);
            var serialized = JsonConvert.SerializeObject(@params);

            Debug.Print(serialized);
        }

        private static void TestRequestDeserialization() {
            const string json = @"[
    { ""name"" : ""My Name"", ""age"" : 10000 },
    10,
    ""2012-04-23T18:25:43.511Z""
]";
            var array = JArray.Parse(json);

            var cls = JsonRpcRequestWrapper.ParamArrayToObject<SimpleClass>(array);

            var serialized = JsonConvert.SerializeObject(cls);

            Debug.Print(serialized);
        }

        private sealed class SimpleClass {

            public SimplePerson Person { get; set; }

            public int Money { get; set; }

            public DateTime Date { get; set; }

            public sealed class SimplePerson {

                public string Name { get; set; }

                public int Age { get; set; }

            }

        }

    }
}
