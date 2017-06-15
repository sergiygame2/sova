using Microsoft.AspNetCore.Hosting;
using SportApp;

namespace Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env) : base(env)
        {
        }
    }
}