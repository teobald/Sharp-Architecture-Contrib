namespace Tests.Configuration
{
    using System.IO;

    public static class Config
    {
        public static string TestDataDir
        {
            get
            {
                return Path.GetFullPath("TestData");
            }
        }
    }
}