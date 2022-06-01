using CodeTranslator.Core.Parser.Components;
using CodeTranslator.Core.Parser.Decomposer;
using CodeTranslator.Core.Parser.Recomposer;
using CodeTranslator.Core.Output;

namespace CodeTranslator.Core.Parser
{
    public class CodeParser : IParser
    {
        private readonly IDecomposer _decomposer;
        private readonly IRecomposer _recomposer;
        private AbstractSyntaxTree<Statement> _cache;

        public CodeParser(IDecomposer decomposer, IRecomposer recomposer)
        {
            _decomposer = decomposer;
            _recomposer = recomposer;
        }

        public AbstractSyntaxTree<Statement> AST => _cache;

        public void Parse(string content) => _cache = _decomposer.Decompose(content);

        public IOutput Translate(string fileName) => _recomposer.Recompose(_cache, fileName);
    }
}
