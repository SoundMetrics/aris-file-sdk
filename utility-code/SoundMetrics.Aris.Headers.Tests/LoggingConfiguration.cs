using Serilog;
using System;

namespace SoundMetrics.Aris.Headers.Tests
{
    public static class LoggingConfiguration
    {
        public static void Configure()
        {
            GC.KeepAlive(doTheWorkOnce.Value); // just ignoring the value.
        }

        private static readonly Lazy<int> doTheWorkOnce = new Lazy<int>(ConfigureTheLogger);

        private static int ConfigureTheLogger()
        {
            // ISO: 2018-02-21T16:09:02+00:00
            var iso8601 = "yyyy-MM-ddTHH:mm:ss.fffzzz";
            var template = "{Timestamp:" + iso8601 + "} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .WriteTo.Console(outputTemplate: template)
                            .CreateLogger();

            return 0;
        }
    }
}
