using System.IO;
using NUnit.Framework;
using SharpArch.Testing.NUnit;
using SharpArchContrib.Core.Logging;
using SharpArchContrib.PostSharp.Logging;

namespace Tests.SharpArchContrib.PostSharp.Logging {
    
    [TestFixture]
    public class DebugLevelTests {
        [Log]
        private void DebugLevelTestsCallThatLogs() {}

        [Log(EntryLevel = LoggingLevel.Info)]
        private void DebugLevelTestsCallThatDoesNotLog() {}

        [Test]
        public void LoggingDebugEntryWorks() {
            DebugLevelTestsCallThatLogs();
            DebugLevelTestsCallThatDoesNotLog();
            string logPath =
                Path.GetFullPath(@"TestData/Tests.SharpArchContrib.PostSharp.Logging.DebugLevelTests.DebugLevel.log");
            File.Exists(logPath).ShouldBeTrue();
            var debugLogInfo = new FileInfo(logPath);
            debugLogInfo.Length.ShouldBeGreaterThan(0);
        }
    }
}