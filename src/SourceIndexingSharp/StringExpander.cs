using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SourceIndexingSharp
{
    /// <summary>
    /// This interfaces replaces environment variables within a string
    /// </summary>
    public class StringExpander : IStringExpander
    {
        private readonly Regex _variableRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringExpander"/> class.
        /// </summary>
        public StringExpander()
        {
            _variableRegex = new Regex(@"\{\{([^}{]+)\}\}", RegexOptions.Compiled);
        }

        /// <summary>
        /// Expand a string, replacing variables with values.
        /// "The system path is {{PATH}}"
        /// </summary>
        /// <param name="value"></param>
        /// <param name="additionalVariables"></param>
        /// <returns></returns>
        public string Expand(string value, Dictionary<string, string> additionalVariables)
        {
            if(additionalVariables == null)
                additionalVariables = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(value))
                return value;

            return _variableRegex.Replace(value, delegate(Match match)
            {
                var key = match.Value.Substring(2, match.Value.Length - 4);
                if (additionalVariables.ContainsKey(key))
                    return additionalVariables[key];
                return Environment.GetEnvironmentVariable(key);
            });
        }
    }
}
