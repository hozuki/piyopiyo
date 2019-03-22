using System;
using JetBrains.Annotations;
using OpenMLTD.Piyopiyo.Bvsp.Client;

namespace ClientTest {
    internal static class Program {

        private static void Main(string[] args) {
            var uri = new Uri("http://localhost:80");

            TestTestAdd(uri);
            TestPlay(uri);
        }

        private static void TestTestAdd([NotNull] Uri uri) {
            var client = new SimulatorService(uri);

            var c = client.TestAdd(20, 45);

            Console.WriteLine("testAdd() result: {0}", c.ToString());
        }

        private static void TestPlay([NotNull] Uri uri) {
            var client = new SimulatorService(uri);

            client.Play();

            Console.WriteLine("play() called");
        }

    }
}
