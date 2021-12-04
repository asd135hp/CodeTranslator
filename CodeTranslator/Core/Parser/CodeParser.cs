using CodeTranslator.Model;
using CodeTranslator.Rules;
using System;
using System.Collections.Generic;

namespace CodeTranslator.Core.Parser
{
    internal class CodeParser : IParser
    {
        public IEnumerable<IRule> Rules { get; set; }
        public CodeTranslationSettings Settings { get; set; }

        public void Parse(string content)
        {
            throw new NotImplementedException();
        }

        public string Translate()
        {
            throw new NotImplementedException();
        }
    }
}
