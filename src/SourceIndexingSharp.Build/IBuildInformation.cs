using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Evaluation;

namespace SourceIndexingSharp.Build
{
    public interface IBuildInformation
    {
        Project Load(string proj, Dictionary<string, string> globalProperties);

        List<string> GetCompileItems(Project proj);
    }
}
