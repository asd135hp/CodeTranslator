using CodeTranslator.Core.Output;
using CodeTranslator.Model;

namespace CodeTranslator.Core
{
    public interface ITranslator
    {
        public IOutput TranslateFile(CodeFile codeFile);
    }
}
