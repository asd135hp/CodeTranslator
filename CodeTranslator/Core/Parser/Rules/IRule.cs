using CodeTranslator.Core.Parser.Components;

namespace CodeTranslator.Core.Parser.Rules
{
    public interface IRule
    {
        bool Match(string content);

        bool Match(params string[] tokens);

        AbstractSyntaxTree<Statement> GetTree(string content);
    }
}
