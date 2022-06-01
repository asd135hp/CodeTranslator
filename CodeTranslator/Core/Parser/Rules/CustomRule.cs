using CodeTranslator.Core.Parser.Components;
using System;

namespace CodeTranslator.Core.Parser.Rules
{
    internal class CustomRule : IRule
    {
        public CustomRule(string rule)
        {

        }

        public AbstractSyntaxTree<Statement> GetTree(string content)
        {
            throw new NotImplementedException();
        }

        public bool Match(string content)
        {
            throw new NotImplementedException();
        }

        public bool Match(params string[] tokens)
        {
            throw new NotImplementedException();
        }
    }
}
