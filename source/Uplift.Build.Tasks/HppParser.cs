using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uplift.Utility.HppParser
{
    // can + operator be added as extenion method? Prob should add class
    // called InternalDSLExtensionMethods or something

    //////////////////////////////////////////////////////////////////////////////////
    /////////// Classes in Parse Tree, which defines *how* to parse

    public abstract class ParseRule
    {
        public string Name { get; set; } //optional

        public static IList<ParseRule> operator +(ParseRule pr1, ParseRule pr2)
        {
            return new List<ParseRule> {pr1, pr2};
        }

        public static IList<ParseRule> operator +(IList<ParseRule> list, ParseRule pr3)
        {
            list.Add(pr3);
            return list;
        }
    }

    public abstract class TerminalSymbol : ParseRule
    {
    }

    // terminal that can be matched using a regex
    public sealed class PatternTerminal : TerminalSymbol
    {
        public string Pattern { get; set; }
    }

    // terminal that is a string literal match
    public class LiteralTerminal : TerminalSymbol
    {
        public LiteralTerminal(string symbol)
        {
            Symbol = symbol;
        }

        public string Symbol { get; set; }
    }

    // a terminal which marks the beginning of a section that can contain
    // nested such symbols and sections, eg {
    public sealed class OpenNestedSectionTerminal : LiteralTerminal
    {
        public OpenNestedSectionTerminal(string symbol) 
            : base(symbol)
        {
        }
    }

    // a terminal which marks the end of a section that can contain
    // nested such symbols and sections, eg }.
    // The parsing semantics of this are to pass everything between
    // balanced opening and closed symbols to the containing expressions
    public sealed class CloseNestedSectionTerminal : LiteralTerminal
    {
        public CloseNestedSectionTerminal(string symbol)
            : base(symbol)
        {
        }
    }

    public sealed class NonTerminal : ParseRule
    {
        public NonTerminal()
        {
            EvaluationOptions = new List<IList<ParseRule>>();
        }

        public IList<IList<ParseRule>> EvaluationOptions { get; set; }
        //public IList<ParseRule> SubItems { get; set; } // TODO: We need to represent "OR" rules somehow, ie alternative parsing paths. List of Lists?
                                                            // would be quite nice to put this into a DSL somehow...
    }

    ////////////////////////////////////////////////////////////////////////
    ///// Abstract Syntax Tree, which contains *what* was parsed

    public sealed class SyntaxItem
    {
        public SyntaxItem()
        {
            SubItems = new OrderedDictionary();
        }

        public string RuleName { get; set; }
        public string Match { get; set; } // optional, not set if non terminal
        public IOrderedDictionary SubItems { get; set; }
    }

    // Ad-hoc parser to parse C++ Header Files for C++ Declarations.
    // This is lighter weight than using a full C++ compiler, and can be contained within a single
    // assembly, with no additional dependencies. The tradeoff is obviously less accurate parsing!
    public class Parser
    {
        public Parser()
        {
            // define a simple C++ declaration expression tree

            var cppDeclaration = new NonTerminal { Name = "CppDeclaration" };

            // What does this do?
            var test = new LiteralTerminal("#include") + new LiteralTerminal("bang") + new LiteralTerminal("jolly");

            /* This is the psuedo BNF for what we're trying to achieve
            
            cppDeclarations => cppDeclaration+ | '' // I think this should be implicit
            cppDeclaration => include
            cppDeclaration => namespace_declaration
            cppDeclaration => comment
            include => '#include' ["<] headerfile [">]
            headerfile => .*?
            namespace_declaration => 'namespace' namespace_name '{' cppDeclarations '}'
            namespace_name => identifier
            identifier => [^\d][a-zA-Z0-9_]+
            comment => '/asterisk' comment_body 'asterisk/' | '//' comment_body \r?\n       
            class_declaration => 'class' identifier '{' class_body '};'
             ... need to define class_body, methods, struct syntax, etc. Also work out how to handle unknown (want to capture blocks of "unknown" in the AST)
            
*/

            // Internal DSL tricks:
            // https://msdn.microsoft.com/en-gb/magazine/ee291514.aspx

            // Use of Extension methods is particularly nice...

        }
    }
}
