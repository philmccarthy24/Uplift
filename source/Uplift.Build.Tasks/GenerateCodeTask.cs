using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Uplift.Build.Tasks
{
    public class GenerateCodeTask : Task
    {
        [Required]
        public string ServiceInterfaceFile { get; set; }

        [Output]
        public ITaskItem[] GeneratedHeaders { get; private set; }

        [Output]
        public ITaskItem[] GeneratedCpps { get; private set; }

        [Output]
        public ITaskItem[] GeneratedResources { get; private set; }

        public override bool Execute()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, "Parsing Service Interface file...");

                var projectDir = Path.GetDirectoryName(BuildEngine.ProjectFileOfTaskNode);
                var genDir = Path.Combine(projectDir, "_generated");

                if (!Directory.Exists(genDir))
                    Directory.CreateDirectory(genDir);

                // for now, just extract the "templates" in their entirety, as they're the original files
                var assembly = Assembly.GetExecutingAssembly();
                var templateFiles = assembly.GetManifestResourceNames();

                foreach (var templateFile in templateFiles)
                {
                    using (var input = assembly.GetManifestResourceStream(templateFile))
                    // get rid of the namespace and res dir information from the resource names
                    using (Stream output = new FileStream(Path.Combine(genDir, templateFile.Replace("Uplift.Build.Tasks.Templates.", "")), FileMode.Create))
                    {
                        input.CopyTo(output);
                    }   
                }

                Log.LogMessage(MessageImportance.High, "Generating WSDL...");

                Log.LogMessage(MessageImportance.High, "Generating Service Status page...");

                Log.LogMessage(MessageImportance.High, "Generating XML serializers...");
                
                var genFiles = Directory.GetFiles(Path.Combine(projectDir, "_generated"));
                GeneratedCpps = genFiles.Where(x => x.EndsWith(".cpp")).Select(x => new TaskItem(x)).ToArray<ITaskItem>();
                GeneratedHeaders = genFiles.Where(x => x.EndsWith(".h")).Select(x => new TaskItem(x)).ToArray<ITaskItem>();

                var genWsdl = new TaskItem(Path.Combine(genDir, "TestService.wsdl"));
                genWsdl.SetMetadata("ResourceId", "904"); // note the resource id must match the one used in the generated C++ code
                var genStatusPage = new TaskItem(Path.Combine(genDir, "StatusPageTemplate.cphtml"));
                genStatusPage.SetMetadata("ResourceId", "905"); // note the resource id must match the one used in the generated C++ code
                GeneratedResources = new TaskItem[2];
                GeneratedResources[0] = genWsdl;
                GeneratedResources[1] = genStatusPage;

                Log.LogMessage(MessageImportance.High, "Code generation complete.");

                return true;
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, true);
            }
            return false;
        }
    }
}
