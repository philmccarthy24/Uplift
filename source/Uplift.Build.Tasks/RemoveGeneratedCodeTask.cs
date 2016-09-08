using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Uplift.Build.Tasks
{
    public class RemoveGeneratedCodeTask : Task
    {
        public override bool Execute()
        {
            Log.LogMessage(MessageImportance.High, "Removing generated files...");
            
            var projectDir = Path.GetDirectoryName(BuildEngine.ProjectFileOfTaskNode);

            try
            {
                Directory.Delete(Path.Combine(projectDir, "_generated"), true);
            }
            catch (DirectoryNotFoundException)
            {
                // ignore
            }

            Log.LogMessage(MessageImportance.High, "Done");

            return true;
        }
    }
}
