using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp
{
    /// <summary>
    /// This interfaces replaces environment variables within a string
    /// </summary>
    public interface IStringExpander
    {
        /// <summary>
        /// Expand a string, replacing variables with values.
        /// "The system path is {{PATH}}" 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="additionalVariables"></param>
        /// <returns></returns>
        string Expand(string value, Dictionary<string, string> additionalVariables);
    }
}
