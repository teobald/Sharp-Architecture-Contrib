namespace Tests.SharpArchContrib.PostSharp.Logging
{
    using System.IO;

    using NUnit.Framework;

    using SharpArch.Testing.NUnit;

    using global::SharpArchContrib.Core.Logging;
    using global::SharpArchContrib.PostSharp.Logging;

    [TestFixture]
    public class DebugLevelTests
    {
        [Test]
        public void LoggingDebugEntryWorks()
        {
            this.DebugLevelTestsCallThatLogs();
            this.DebugLevelTestsCallThatDoesNotLog();
            string logPath =
                Path.GetFullPath(@"TestData/Tests.SharpArchContrib.PostSharp.Logging.DebugLevelTests.DebugLevel.log");
            File.Exists(logPath).ShouldBeTrue();
            var debugLogInfo = new FileInfo(logPath);
            debugLogInfo.Length.ShouldBeGreaterThan(0);
        }

        [Log(EntryLevel = LoggingLevel.Info)]
        private void DebugLevelTestsCallThatDoesNotLog()
        {
        }

        [Log]
        private void DebugLevelTestsCallThatLogs()
        {
        }
    }
}