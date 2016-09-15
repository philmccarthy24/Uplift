using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Uplift.VSExtensions
{
    public class NewProjectWizard : IWizard
    {
        private string _templateDir;
        private string _projectDir;

        // This method is called before opening any item that 
        // has the OpenInEditor attribute.
        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
        }

        // This method is only called for item templates,
        // not for project templates.
        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
        }

        // This method is called after the project is created.
        public void RunFinished()
        {
            // copy the Uplift.Build.Tasks assembly from the vsix extraction area to the new project location
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var sourceTasksAssembly = Path.Combine(assemblyPath, "Uplift.Build.Tasks.dll");
            File.Copy(sourceTasksAssembly, Path.Combine(_projectDir, "Uplift.Build.Tasks.dll"));

            // copy the pugixml files over, that aren't included in the vcxproj (and so won't be auto-copied)
            var destPugiDir = Path.Combine(_projectDir, "pugixml");
            if (!Directory.Exists(destPugiDir))
                Directory.CreateDirectory(destPugiDir);
            var sourcePugiDir = Path.Combine(_templateDir, "pugixml");
            File.Copy(Path.Combine(sourcePugiDir, "pugiconfig.hpp"), Path.Combine(destPugiDir, "pugiconfig.hpp"));
            File.Copy(Path.Combine(sourcePugiDir, "pugixml.hpp"), Path.Combine(destPugiDir, "pugixml.hpp"));
            File.Copy(Path.Combine(sourcePugiDir, "pugixml.cpp"), Path.Combine(destPugiDir, "pugixml.cpp"));
            File.Copy(Path.Combine(sourcePugiDir, "readme.txt"), Path.Combine(destPugiDir, "readme.txt"));
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            _projectDir = replacementsDictionary["$destinationdirectory$"];
            try
            {
                // Display a form to the user. The form collects 
                // input for the custom message.
                var inputForm = new WizardForm();
                if (inputForm.ShowDialog() != DialogResult.OK)
                    throw new WizardBackoutException();

                // we need to generate C++ style namespace declarations from the dotted notation given by the user
                var sb = new StringBuilder();
                var sb2 = new StringBuilder();
                var namespaces = inputForm.ServiceNamespace.Split(new char[] { '.' });
                foreach (var ns in namespaces)
                {
                    sb.AppendFormat("namespace {0}\r\n{{\r\n", ns);
                    sb2.Append("}\r\n");
                }

                // Add custom parameters.
                replacementsDictionary.Add("$serviceinterfacename$", inputForm.ServiceInterfaceName);
                replacementsDictionary.Add("$serviceimplclassname$", inputForm.ServiceImplementationClassName);
                replacementsDictionary.Add("$beginnamespace$", sb.ToString());
                replacementsDictionary.Add("$endnamespace$", sb2.ToString());
                replacementsDictionary.Add("$servicenamespace$", inputForm.ServiceNamespace.Replace(".", "::"));
                replacementsDictionary.Add("$iisexpressport$", inputForm.IISExpressPort.ToString());
                replacementsDictionary.Add("$iisexpressserviceroot$", inputForm.IISExpressServiceRoot);

                _templateDir = Path.GetDirectoryName((string)customParams[0]);
            }
            catch (Exception ex)
            {
                // Clean up the template that was written to disk
                if (Directory.Exists(_projectDir))
                {
                    Directory.Delete(_projectDir, true);
                }
                throw;
            }
        }

        // This method is only called for item templates,
        // not for project templates.
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
