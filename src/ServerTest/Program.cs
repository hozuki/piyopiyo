using System;
using OpenMLTD.Piyopiyo.Bvsp.Server;

namespace ServerTest {
    internal static class Program {

        private static void Main(string[] args) {
            var server = new SimulatorServer(80, new SimulatorImpl());

            server.Start();

            Console.WriteLine("Server started.");

            Console.ReadKey(true);

            server.Stop();

            Console.WriteLine("Server stopped.");
        }

    }
}
