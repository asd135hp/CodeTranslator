using System;
using CodeTranslator.Core.Parser.Components;

namespace CodeTranslator.Core.Parser.Decomposer
{
    public class Extractor : LanguageInfoLoader, IDecomposer
    {
        public Extractor()
        {

        }



        public AbstractSyntaxTree<Statement> Decompose(string content)
        {
            throw new NotImplementedException();
        }

    }
}
