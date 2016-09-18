
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TypeAndName = System.Collections.Generic.Dictionary<string, string>;

namespace Uplift.HppParser
{
    public class Function
    {
        //todo how to make into proper object (do we need to for wsdl?)
        public string ReturnType { get; }
        public string Name { get; }
        public TypeAndName Arguments { get; } = new TypeAndName();

        public Function(string name, string returnType = "void") // todo maybe covert null to void
        {
            Name = name;
            ReturnType = returnType;
        }

        public Function(string name, TypeAndName arguments, string returnType = "void")
        {
            Name = name;
            ReturnType = returnType;
            Arguments = arguments;
        }

        public override string ToString()
        {
            var arguments = Arguments.Select(kv => kv.Key.ToString() + " " + kv.Value.ToString());
            return $"{ReturnType} {Name}({string.Join(", ", arguments)});";
        }
    }
    
    public interface IObjectDef
    {
        string Name { get; }
        IList<Function> PublicFunctions { get;  }
    }

    public class ObjectDef : IObjectDef
    {
        public string Name { get; }
        public IList<Function> PublicFunctions { get; set; } = new List<Function>(); // todo make readonly

        public bool IsClass {get;}

        public ObjectDef(string name, bool isClass)
        {
            Name = name;
            IsClass = isClass;
        }
    }

    public static class HppParser
    {
        const string startBlockChars = "/*";
        const string endBlockChars = "*/";
        const string lineCommentChars = "//";
        const char openingBraceChar = '{';
        const char closingBraceChar = '}';
        const string classSpec = "class";
        const string structSpec = "struct";

        public static List<IObjectDef> ParseFile(string file)
        {
            // todo do as whole file not lines
            // todo find service classes might be easier to do 
            // by just looking for signature
            var fileContents = File.ReadAllLines(file);
            var cleanedLines = fileContents.RemoveBlankLines()
                                           .RemoveBlockComments()
                                           .RemoveForwardSlashComments()
                                           .RemoveBlankLines();

            return GetObjectDefs(string.Join("\n", cleanedLines));
        }
        
        private static IEnumerable<string> RemoveBlankLines(this IEnumerable<string> lines) => 
            lines.Where(line => !string.IsNullOrWhiteSpace(line));

        // todo easier to do with regex?
        private static IEnumerable<string> RemoveForwardSlashComments(this IEnumerable<string> lines)
        {
            return lines.Select(line => 
            {
                if (!line.Contains(lineCommentChars))
                    return line;
                return line.Substring(0, line.IndexOf(lineCommentChars));
            });
        }

        // todo easier to do with regex?
        private static IEnumerable<string> RemoveBlockComments(this IEnumerable<string> lines)
        {
            var inCommentBlock = false;
            var linesWithoutBlockComment = new List<string>();
            foreach (var line in lines)
            {
                string lineToAdd = null;

                if (line.Contains(startBlockChars) && !inCommentBlock)
                {
                    inCommentBlock = true;
                    lineToAdd = line.Substring(0, line.IndexOf(startBlockChars));
                }
                else if (inCommentBlock && line.Contains(endBlockChars))
                {
                    inCommentBlock = false;
                    lineToAdd = line.Substring(line.IndexOf(endBlockChars) + endBlockChars.Length);
                }
                else if(!inCommentBlock)
                {
                    lineToAdd = line;
                }

                if (!string.IsNullOrWhiteSpace(lineToAdd))
                    linesWithoutBlockComment.Add(lineToAdd);
            }

            return linesWithoutBlockComment;
        }

        private static List<IObjectDef> GetObjectDefs(string strippedFileContent)
        {
            var objectDefs = new List<IObjectDef>();
            const string nameGroup = "Name";
            const string defTypeGroup = "DefType";

            var regexPattern = $@"(?<{defTypeGroup}>{classSpec}|{structSpec}) (?<{nameGroup}>\w+)";
            var objectDefMatches = Regex.Matches(strippedFileContent, regexPattern);

            foreach(Match objectDefMatch in objectDefMatches)
            {
                var objectDef = new ObjectDef(
                    objectDefMatch.Groups[nameGroup].Value,
                    isClass: objectDefMatch.Groups[defTypeGroup].Value == classSpec);
                var objectContent = GetObjectContent(strippedFileContent, objectDefMatch);
                var functions = GetPublicFunctions(objectContent, objectDef.IsClass);
                objectDef.PublicFunctions = functions;
            }

            return objectDefs;
        }

        private static List<Function> GetPublicFunctions(string objectContent, bool isClass)
        {


            return GetFunctions(objectContent);
        }

        private static List<Function> GetFunctions(string objectContent)
        {
            const string returnTypeGroup = "ReturnType";
            const string functionNameGroup = "FunctionName";
            const string argumentsGroup = "Arguments";

            var regexPattern = $@"(?<{returnTypeGroup}>\S+) (?<{functionNameGroup}>\w+)\((?<{argumentsGroup}>[^\)]*)\)"; // get long long
            var functionMatches = Regex.Matches(objectContent, regexPattern);

            var functions = new List<Function>();
            foreach (Match match in functionMatches)
            {
                var function = new Function(
                    match.Groups[functionNameGroup].Value,
                    SplitArguments(match.Groups[argumentsGroup].Value),
                    match.Groups[returnTypeGroup].Value);
                functions.Add(function);
            }

            return functions;
        }

        private static TypeAndName SplitArguments(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                return new TypeAndName();
            var argumentTypeAndNames = value.Split(',');

            const string TypeGroup = "Type";
            const string NameGroup = "Name";
            var argumentRegex = $@"^(?<{TypeGroup}>.+) (?<{NameGroup}>\w*)$";

            var argumentDict = new TypeAndName();
            foreach (var argumentTypeAndName in argumentTypeAndNames)
            {
                var match = Regex.Match(argumentTypeAndName, argumentRegex);
                argumentDict.Add(match.Groups[TypeGroup].Value, match.Groups[NameGroup].Value);
            }

            return argumentDict;
        }

        private static string GetObjectContent(string strippedFileContent, Match objectDefMatch)
        {
            var startOfContentIndex = strippedFileContent.IndexOf("{", objectDefMatch.Index + objectDefMatch.Length) + 1; //1 length of { todo calculate
            var endOfContentIndex = strippedFileContent.IndexOf("};", objectDefMatch.Index + objectDefMatch.Length);
            return strippedFileContent.Substring(
                startOfContentIndex,
                endOfContentIndex - startOfContentIndex);
        }
    }
}
