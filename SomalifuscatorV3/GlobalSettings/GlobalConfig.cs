using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomalifuscatorV3.GlobalSettings
{
    public static class GlobalConfig
    {
        private static readonly Dictionary<string, string> variableAndDefinitions = new Dictionary<string, string>();

        public static void AddOrUpdateVariable(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

            variableAndDefinitions[key] = value;
        }

        public static string? GetVariable(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

            variableAndDefinitions.TryGetValue(key, out string? value);
            return value;
        }

        public static bool ContainsVariable(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

            return variableAndDefinitions.ContainsKey(key);
        }

        public static bool RemoveVariable(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

            return variableAndDefinitions.Remove(key);
        }
    }
}
