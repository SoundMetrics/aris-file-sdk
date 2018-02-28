// Copyright 2018 Sound Metrics Corp. All Rights Reserved.

namespace CheckArisFile
{
    class Program
    {
        private const string ProgramName = "CheckArisFile";

        static void Main(string[] args)
        {
            Core.Checker.CheckArisFile(args, ProgramName, Core.Checker.ConfirmTruncation);
        }
    }
}
