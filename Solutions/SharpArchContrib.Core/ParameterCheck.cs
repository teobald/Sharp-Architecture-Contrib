namespace SharpArchContrib.Core
{
    using System;
    using System.Collections.Generic;
    using SharpArch.Domain;

    public static class ParameterCheck
    {
        public static void DictionaryContainsKey(
            IDictionary<string, object> dictionary, string dictionaryName, string key)
        {
            ParameterRequired(dictionary, "dictionary");
            StringRequiredAndNotEmpty(key, "key");
            StringRequiredAndNotEmpty(dictionaryName, "dictionaryName");

            if (!dictionary.ContainsKey(key))
            {
                throw new ArgumentException(
                    string.Format("Dictionary parameter {0} must contain an entry with key value {1}", dictionaryName, key));
            }
        }

        public static void DictionaryContainsKey(
            IDictionary<string, string> dictionary, string dictionaryName, string key)
        {
            ParameterRequired(dictionary, "dictionary");
            StringRequiredAndNotEmpty(key, "key");
            StringRequiredAndNotEmpty(dictionaryName, "dictionaryName");

            if (!dictionary.ContainsKey(key))
            {
                throw new ArgumentException(
                    string.Format("Dictionary parameter {0} must contain an entry with key value {1}", dictionaryName, key));
            }
        }

        public static void ParameterRequired(object parameter, string parameterName)
        {
            ParameterNameRequired(parameterName);

            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName, GetParameterRequiredErrorMessage(parameterName));
            }
        }

        public static void StringRequiredAndNotEmpty(string parameter, string parameterName)
        {
            ParameterNameRequired(parameterName);

            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentException(GetParameterRequiredErrorMessage(parameterName), parameterName);
            }
        }

        private static string GetParameterRequiredErrorMessage(string parameterName)
        {
            return string.Format("The parameter {0} is required.", parameterName);
        }

        private static void ParameterNameRequired(string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentException(GetParameterRequiredErrorMessage("parameterName"), "parameterName");
            }
        }
    }
}