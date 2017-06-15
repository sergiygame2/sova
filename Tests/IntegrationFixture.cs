using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.IO;
using Xunit;

namespace Tests
{
    public class IntegrationFixture : IDisposable
    {
        public TestBrowser Browser { get; }

        public IntegrationFixture()
        {
            string current = Directory.GetCurrentDirectory();
            string parent = Directory.GetParent(current).FullName;
            string webRootPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(parent).FullName).FullName).FullName, "SportApp");

            Console.WriteLine("Fixture start");

            var builder = new WebHostBuilder()
                .UseEnvironment("Production")
                .UseContentRoot(webRootPath)
                .UseStartup<TestStartup>();
            Console.WriteLine("Fixture start server");
            var server = new TestServer(builder);

            Console.WriteLine("Fixture start browser");
            Browser = new TestBrowser(server);

        }

        public void Dispose()
        {
            Console.WriteLine("IntegrationFixture disposed");
        }
    }

    [CollectionDefinition("IntegrationTests")]
    public class DatabaseCollection : ICollectionFixture<IntegrationFixture>
    {

    }
}
