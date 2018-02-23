// Copyright 2018 Sound Metrics Corp. All Rights Reserved.

using Serilog;

namespace SoundMetrics.Aris.Headers.UT
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigureLogging();

            // tests!
        }

        private static void ConfigureLogging()
        {
            // ISO 8601-ish: 2018-02-21T16:09:02+00:00
            var iso8601 = "yyyy-MM-ddTHH:mm:ss.fffzzz";
            var template = "{Timestamp:" + iso8601 + "} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .WriteTo.Console(outputTemplate: template)
                            .CreateLogger();
        }
    }
}
