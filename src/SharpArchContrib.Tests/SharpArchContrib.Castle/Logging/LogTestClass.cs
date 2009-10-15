using SharpArchContrib.Castle.Logging;

namespace Tests.SharpArchContrib.Castle.Logging {
    public class LogTestClass : ILogTestClass {
        #region ILogTestClass Members

        [Log]
        public int Method(string name, int val) {
            return val;
        }

        [Log]
        public virtual int VirtualMethod(string name, int val) {
            return val;
        }

        public virtual int NotLogged(string name, int val) {
            return val;
        }

        #endregion
    }
}