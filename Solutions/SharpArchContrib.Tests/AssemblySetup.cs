namespace Tests
{
    using System.IO;

    using log4net.Config;

    using NUnit.Framework;

    using Tests.Configuration;

    [SetUpFixture]
    public class AssemblySetup
    {
        [SetUp]
        public void SetUp()
        {
            this.InitializeDirectories();
            this.InitializeLog4Net();
            this.InitalizeServiceLocator();
        }

        private void InitalizeServiceLocator()
        {
            ServiceLocatorInitializer.Init();
        }

        private void InitializeDirectories()
        {
            if (Directory.Exists(Config.TestDataDir))
            {
                Directory.Delete(Config.TestDataDir, true);
            }

            Directory.CreateDirectory(Config.TestDataDir);
        }

        private void InitializeLog4Net()
        {
            XmlConfigurator.Configure();
        }
    }
}