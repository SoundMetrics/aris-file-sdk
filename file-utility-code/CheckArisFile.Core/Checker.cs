// Copyright 2018 Sound Metrics Corp. All Rights Reserved.

using SoundMetrics.Aris.Files;
using System;


namespace CheckArisFile.Core
{
    using static MatchResult;
    using static Corruption;
    using Result = SoundMetrics.Aris.Files.Result<string, string>;
    using ConsentResult = SoundMetrics.Aris.Files.Result<Corruption.ConsentResponse, string>;

    public static class Checker
    {
        public static Result<object, object> CheckFileForProblems { get; private set; }

        public static void CheckArisFile(string[] args, string programName, ConfirmTruncationFn confirmTruncation)
        {
            var result =
                GetArgs(args)
                    .Bind(filePath => ValidateArgs(filePath)
                        .Bind(path => DoTheWork(path, confirmTruncation)));
            MatchVoid(result,
                onOk: msg => Console.WriteLine($"Finished: {msg}."),
                onError: msg => {
                    Console.Error.WriteLine();
                    Console.Error.WriteLine($"ERROR: {msg}.");
                    PrintUsage(programName);
                });
        }

        private static void PrintUsage(string programName)
        {
            Console.WriteLine($"USAGE: {programName} <filepath>");
            Console.WriteLine();
            Console.WriteLine("    <filepath> must refer to an ARIS recording file.");
            Console.WriteLine();
        }

        internal static Result GetArgs(string[] args)
        {
            if (args.Length == 0)
            {
                return Result.Error("No file path was given");
            }
            else if (args.Length == 1)
            {
                var filepath = args[0];
                return Result.Ok(filepath);
            }
            else
            {
                return Result.Error("Too many arguments");
            }
        }

        internal static Result ValidateArgs(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                return Result.Ok(filePath);
            }
            else
            {
                return Result.Error($"File not found: {filePath}");
            }
        }

        internal static Result DoTheWork(string filePath, ConfirmTruncationFn confirmTruncation)
        {
            Console.WriteLine($"Checking file: {filePath}");

            return Match(CheckFileForProblems(filePath),
                    onOk: check => {
                    if (check.IsCorruptionDetected)
                    {
                        return Match(CorrectFileProblems(filePath, ConfirmTruncation),
                                onOk: check2 => {
                                    if (check2.IsCorruptionDetected)
                                    {
                                        return Result.Ok("The file still has problems");
                                    }
                                    else
                                    {
                                        return Result.Ok("Fixes were made to the file");
                                    }
                                },
                                onError: errorInfo => Result.Error(errorInfo.Text));
                        }
                        else
                        {
                            return Result.Ok("No corruption detected");
                        }
                    },
                    onError: errorInfo => Result.Error(errorInfo.Text));
        }

        // This functionality is not hard-coded into the logic so we can test more easily.
        public static ConsentResponse ConfirmTruncation(uint newFrameCount, uint oldFrameCount)
        {
            bool waitingForResponse = true;
            ConsentResponse userResponse = ConsentResponse.PleaseDont;

            do
            {
                Console.WriteLine("Corruption was detected.");

                if (newFrameCount != oldFrameCount)
                {
                    Console.WriteLine($"File will be truncated from {oldFrameCount} to {newFrameCount}");
                }

                Console.WriteLine("Do you wish to correct the file, possibly leaving it truncated? y/n");

                MatchVoid(CheckUserResponse(Console.ReadLine()),
                    onOk: answer => {
                        userResponse = answer;
                        waitingForResponse = false;
                    },
                    onError: msg => Console.Error.WriteLine(msg));
            } while (waitingForResponse);

            return userResponse;
        }

        internal static ConsentResult CheckUserResponse(string userResponse)
        {
            var cleanResponse = userResponse == null ? null : userResponse.Trim();

            if (String.Compare(cleanResponse, "y", ignoreCase: true) == 0
                || String.Compare(cleanResponse, "yes", ignoreCase: true) == 0)
            {
                return ConsentResult.Ok(ConsentResponse.PleaseFix);
            }
            else if (String.Compare(cleanResponse, "n", ignoreCase: true) == 0
                || String.Compare(cleanResponse, "no", ignoreCase: true) == 0)
            {
                return ConsentResult.Ok(ConsentResponse.PleaseDont);
            }
            else
            {
                return ConsentResult.Error("Unexpected response");
            }
        }
    }
}
