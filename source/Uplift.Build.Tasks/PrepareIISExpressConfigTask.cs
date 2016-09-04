using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Uplift.Build.Tasks
{
    public class PrepareIISExpressConfigTask : Task
    {
        [Required]
        public string SourceIISExpressConfig { get; set; }

        [Required]
        public string TargetDir { get; set; }

        public override bool Execute()
        {
            Log.LogMessage(MessageImportance.High, "Preparing IIS Express config...");

            string content = File.ReadAllText(SourceIISExpressConfig);

            // very simple template - just a single place that needs replacement!
            content = content.Replace("{REPLACE_ME}", TargetDir);
            var targetPath = Path.Combine(TargetDir, Path.GetFileName(SourceIISExpressConfig));

            File.WriteAllText(targetPath, content);

            Log.LogMessage(MessageImportance.High, "IIS Express setup ok.");

            return true;
        }
    }
}
