using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CoreAPIDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseSentry(o =>
                  {
                      // Tells which project in Sentry to send events to:
                      o.Dsn = "https://xxxxxxxxxxxxxxxxx@xxxxxxxxxx.xxxxxxx.xxxxxx.xx/xxxxxxxxxxxxx";
                      // When configuring for the first time, to see what the SDK is doing:
                      o.Debug = true;
                      // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
                      // We recommend adjusting this value in production.
                      o.TracesSampleRate = 0.5;
                      // Opt-in to send things like UserId and UserName if a user is logged-in
                      o.SendDefaultPii = true;
                  })
                 .ConfigureKestrel(serverOptions =>
                 {
                     serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
                 })
               .UseStartup<Startup>();

    }
}
