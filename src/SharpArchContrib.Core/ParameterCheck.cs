using SharpArch.Core;

namespace SharpArchContrib.Core {
    public static class ParameterCheck {
        private static string GetParameterRequiredErrorMessage(string parameterName) {
            return string.Format("The parameter {0} is required.", parameterName);
        }

        private static void ParameterNameRequired(string parameterName) {
            Check.Require(!string.IsNullOrEmpty(parameterName), GetParameterRequiredErrorMessage("parameterName"));
        }

        public static void ParameterRequired(object parameter, string parameterName) {
            ParameterNameRequired(parameterName);

            Check.Require(parameter != null, GetParameterRequiredErrorMessage(parameterName));
        }

        public static void StringRequiredAndNotEmpty(string parameter, string parameterName) {
            ParameterNameRequired(parameterName);

            Check.Require(!string.IsNullOrEmpty(parameter), GetParameterRequiredErrorMessage(parameterName));
        }
    }
}