using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Evaluation;

namespace SourceIndexingSharp.Build
{
    public class BuildInformation : IBuildInformation
    {
        public BuildInformation()
        {
            
        }

        public Project Load(string proj, Dictionary<string, string> globalProperties)
        {
            var projectCollection = new ProjectCollection();
            return new Project(proj, globalProperties, null, projectCollection);
        }

        public List<string> GetCompileItems(Project proj)
        {
            return proj.Items.Where(x => x.ItemType == "Compile").Select(x => x.EvaluatedInclude).ToList();
        } 
    }
}
