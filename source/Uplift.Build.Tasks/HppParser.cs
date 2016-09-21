using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uplift.Utility
{
    //////////////////////////////////////////////////////////////////////////////////
    /////////// Classes in Parse Tree / Expression Tree, which defines *how* to parse

    public abstract class ExpressionItem
    {
        public string Name { get; set; } //optional
    }

    public abstract class TerminalSymbol : ExpressionItem
    {
    }

    public sealed class PatternTerminal : TerminalSymbol
    {
        public string Pattern { get; set; }
    }

    public sealed class LiteralTerminal : TerminalSymbol
    {
        public string Symbol { get; set; }
    }

    public sealed class OpenNestedSectionTerminal : LiteralTerminal
    {
    }

    // parsing semantics: pass everything between balanced opening and closed symbols to
    // the containing expressions
    public sealed class CloseNestedSectionTerminal : LiteralTerminal
    {
    }

    public sealed class NonTerminal : ExpressionItem
    {
        public NonTerminal()
        {
            SubItems = new OrderedDictionary();
        }

        public IOrderedDictionary SubItems { get; set; }
    }

    ////////////////////////////////////////////////////////////////////////
    ///// Classes in Abstract Syntax Tree, which contains *what* was parsed

    public sealed class TerminalSymbolMatch
    {
        public TerminalSymbol MatchingRule { get; set; }
        public string Match { get; set; }
    }

    public sealed class NonTerminalMatch
    {
        public NonTerminalMatch()
        {
            SubItems = new OrderedDictionary();
        }

        public NonTerminal MatchingRule { get; set; }
        public IOrderedDictionary SubItems { get; set; }
        /////////////////////////////////////////////////////////// missing stuff here. Need names? ...
    }

    public class HppParser
    {

    }
}
