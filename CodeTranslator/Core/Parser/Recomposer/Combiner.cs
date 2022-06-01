using System;
using CodeTranslator.Core.Parser.Components;
using CodeTranslator.Core.Output;

namespace CodeTranslator.Core.Parser.Recomposer
{
    public class Combiner : LanguageInfoLoader, IRecomposer
    {
        public Combiner()
        {

        }

        public IOutput Recompose(AbstractSyntaxTree<Statement> ast, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
