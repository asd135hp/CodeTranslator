using CodeTranslator.Core.Output;
using CodeTranslator.Core.Model;

namespace CodeTranslator.Core
{
    public interface ITranslator
    {
        public GenericOutput TranslateFile(CodeFile codeFile);
    }
}
