using CodeTranslator.Core.Parser.Components;
using CodeTranslator.Core.Output;

namespace CodeTranslator.Core.Parser.Recomposer
{
    public interface IRecomposer
    {
        /// <summary>
        /// Recompose abstract syntax tree (AST) into another language,
        /// following the provided rules from parser langauge info
        /// </summary>
        /// <param name="ast"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        IOutput Recompose(AbstractSyntaxTree<Statement> ast, string fileName);
    }
}
