using CodeTranslator.Core.Parser.Components;

namespace CodeTranslator.Core.Parser.Decomposer
{
    public interface IDecomposer
    {
        /// <summary>
        /// Decompose the raw content into an abstract syntax tree (AST),
        /// following the provided rules from parser langauge info
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        AbstractSyntaxTree<Statement> Decompose(string content);
    }
}
