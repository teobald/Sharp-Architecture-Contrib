namespace Tests.SharpArchContrib.Castle.Logging
{
    using System;
    using System.IO;

    using global::Castle.DynamicProxy;

    using Microsoft.Practices.ServiceLocation;

    using NUnit.Framework;

    using SharpArch.Testing.NUnit;

    using global::SharpArchContrib.Castle.Logging;
    using global::SharpArchContrib.Core.Logging;

    [TestFixture]
    public class LoggingTests
    {
        [Test]
        public void LoggingDebugEntryWorks()
        {
            this.TryLogging();
            this.TryLoggingViaProxy();
            var logPath =
                Path.GetFullPath(@"TestData/Tests.SharpArchContrib.Castle.Logging.DebugLevelTests.DebugLevel.log");
            File.Exists(logPath).ShouldBeTrue();
            var debugLogInfo = new FileInfo(logPath);
            debugLogInfo.Length.ShouldBeGreaterThan(0);
        }

        private void TryLogging()
        {
            var testClass = ServiceLocator.Current.GetInstance<ILogTestClass>();
            testClass.Method("Tom", 1);
            testClass.VirtualMethod("Bill", 2);
            testClass.NotLogged("Philly", 3);
            Assert.Throws<Exception>(() => testClass.ThrowException());
        }

        private void TryLoggingViaProxy()
        {
            var generator = new ProxyGenerator();
            var testLogger2 =
                generator.CreateClassProxy<TestLogger2>(
                    ServiceLocator.Current.GetInstance<IInterceptor>("LogInterceptor"));
            testLogger2.GetMessage("message1");
            testLogger2.GetMessageVirtual("message2");
            testLogger2.GetMessageNotLogged("message3");
        }

        private void TryLoggingViaForwardedType()
        {
            var testClass = ServiceLocator.Current.GetInstance<IAmForwarded>();
            testClass.MethodFromForwarded();
        }

        private string ReadLogFile(string logPath)
        {
            using (var fs = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        public class TestLogger2
        {
            [Log]
            public string GetMessage(string message)
            {
                return message;
            }

            public virtual string GetMessageNotLogged(string message)
            {
                return message;
            }

            [Log(EntryLevel = LoggingLevel.Info)]
            public virtual string GetMessageVirtual(string message)
            {
                return message;
            }
        }

        // issue#1 https://github.com/sharparchitecture/Sharp-Architecture-Contrib/issues#issue/1
        [Test]
        public void LoggingViaForwardedTypeWorks() 
        {
            var logPath =
                Path.GetFullPath(@"TestData/Tests.SharpArchContrib.Castle.Logging.DebugLevelTests.DebugLevel.log");
            TryLoggingViaForwardedType();
            File.Exists(logPath).ShouldBeTrue();
            var debugLogInfo = new FileInfo(logPath);
            debugLogInfo.Length.ShouldBeGreaterThan(0);
            var logFileContents = ReadLogFile(logPath);
            string messageThatShouldBeLoggedOnce = "MethodFromForwarded()";
            int firstOccurenceLocation = logFileContents.IndexOf(messageThatShouldBeLoggedOnce);
            int lastOccurenceLocation = logFileContents.LastIndexOf(messageThatShouldBeLoggedOnce);
            firstOccurenceLocation.ShouldBeGreaterThan(0);
            firstOccurenceLocation.ShouldEqual(lastOccurenceLocation);
        }
    }
}