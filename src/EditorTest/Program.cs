using System;
using OpenMLTD.Piyopiyo.Editor;

namespace EditorTest {
    internal static class Program {

        private static void Main(string[] args) {
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

    }
}
