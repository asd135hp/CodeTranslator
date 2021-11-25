using CodeTranslator.Core.Output;
using CodeTranslator.Model;

namespace CodeTranslator.Core
{
    public interface ITranslator
    {
        public GenericOutput TranslateFile(CodeFile codeFile);
    }
}
