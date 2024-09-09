// Copyright 2024 Sound Metrics Corp. All Rights Reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SoundMetrics.Aris.BeamWidths
{
    internal static class BeamWidthsFileLoader
    {
        public static BeamInfo[] LoadBeamWidthsFile(string beamWidthsSetName)
        {
            if (string.IsNullOrWhiteSpace(beamWidthsSetName))
            {
                throw new System.ArgumentException($"'{nameof(beamWidthsSetName)}' cannot be null or whitespace.", nameof(beamWidthsSetName));
            }

            var fileFolder =
                Path.GetDirectoryName(typeof(BeamWidthsFileLoader).Assembly.Location);
            const string subfolder = "beam-width-metrics";
            var filePath = Path.Combine(fileFolder, subfolder, beamWidthsSetName + ".h");

            Debug.WriteLine($"Loading widths set [{beamWidthsSetName}] from file [{filePath}]");

            var beamInfos =
                File.ReadLines(filePath)
                    .Select(s => s.Trim())
                    .Where(IsNotAComment)
                    .Select(SplitLine)
                    .ToArray();

            return beamInfos;
        }

        private static bool IsNotAComment(string line)
            => !(line.StartsWith("//") || line.StartsWith("#"));

        private static BeamInfo SplitLine(string line)
        {
            // Example line:
            //      DEFINE_BEAMWIDTH3(47, 7.339, 7.345, 7.487)

            char[] delims = [',', ' ', '\t', '(', ')']; // Build machine said inline was ambiguous.
            var splits = line.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            return splits switch
            {
                ["DEFINE_BEAMWIDTH3", var beamNumber, var center, var left, var right] =>
                    MakeBeamInfo(beamNumber, center, left, right),
                _ => throw new Exception($"Invalid format in input: [{line}]")
            };
        }

        private static BeamInfo MakeBeamInfo(string beamNumber, string center, string left, string right)
        {
            return new BeamInfo
            {
                BeamNumber = uint.Parse(beamNumber),
                Center = float.Parse(center),
                Left = float.Parse(left),
                Right = float.Parse(right),
            };
        }
    }
}
