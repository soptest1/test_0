using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

using System.Net.WebSockets;

namespace Test_0
{
    class MainClass
    {
        static readonly IEnumerable<string> s_urlList = new string[]
        {
            "https://docs.microsoft.com",
            "https://docs.microsoft.com/aspnet/core",
            "https://docs.microsoft.com/azure",
            "https://docs.microsoft.com/azure/devops",
            "https://docs.microsoft.com/dotnet",
            "https://docs.microsoft.com/dynamics365",
            "https://docs.microsoft.com/education",
            "https://docs.microsoft.com/enterprise-mobility-security",
            "https://docs.microsoft.com/gaming",
            "https://docs.microsoft.com/graph",
            "https://docs.microsoft.com/microsoft-365",
            "https://docs.microsoft.com/office",
            "https://docs.microsoft.com/powershell",
            "https://docs.microsoft.com/sql",
            "https://docs.microsoft.com/surface",
            "https://docs.microsoft.com/system-center",
            "https://docs.microsoft.com/visualstudio",
            "https://docs.microsoft.com/windows",
            "https://docs.microsoft.com/xamarin"
        };
        static HttpClient client = new HttpClient();
        public static async Task<int> Foo(string uri)
        {
            Stopwatch stop = Stopwatch.StartNew();
            Console.WriteLine($"Foo({uri}), thid: {Thread.CurrentThread.ManagedThreadId}");
            //CancellationToken ct = new CancellationToken();
            //ClientWebSocket ws = new ClientWebSocket();
            //await ws.ConnectAsync(new Uri(uri), ct);
            //await Task.Delay(1000);
            string msg = await client.GetStringAsync(uri);
            stop.Stop();
            Console.WriteLine($"Elapsed \"{uri,-60}\" { stop.Elapsed}, {msg.Length}, thid: {Thread.CurrentThread.ManagedThreadId}");
            return msg.Length;
        }
        public static async Task Main(string[] args)
        {
            Stopwatch stop = Stopwatch.StartNew();
            List<Task<int>> arr = new List<Task<int>>();
            foreach (string uri in s_urlList)
            {
                Task<int> task = Foo(uri);
                arr.Add(task);
                //await task;
            }
            while (arr.Count != 0)
            {
                Task<int> task = await Task.WhenAny(arr.ToArray());
                arr.Remove(task);
                await task;
            }
            stop.Stop();
            Console.WriteLine($"Done: {stop.Elapsed}");
            //comment
        }
    }
}
