using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uplift.Utility;

namespace Uplift.Build.Tasks
{
    public class EmbedResourcesTask : Task
    {
        [Required]
        public ITaskItem[] FilesToEmbed { get; set; }

        [Required]
        public string TargetExe { get; set; }

        public override bool Execute()
        {
            Log.LogMessage(MessageImportance.High, "Embedding custom data resources...");

            try
            {
                var resEmbedder = new ResourceEmbedder();

                foreach (var item in FilesToEmbed)
                {
                    int resId = int.Parse(item.GetMetadata("ResourceId"));
                    resEmbedder.AddDataResource(TargetExe, item.ToString(), resId);
                    Log.LogMessage(MessageImportance.High, string.Format("\tAdded {0} to {1} with ResId {2}", item.ToString(), TargetExe, resId));
                }
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
                return false;
            }

            Log.LogMessage(MessageImportance.High, "Custom data resources embedded.");

            return true;
        }
    }
}
