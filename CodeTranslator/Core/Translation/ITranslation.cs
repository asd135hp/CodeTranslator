using CodeTranslator.Core.Model;
using CodeTranslator.Core.Output;

namespace CodeTranslator.Core.Translation
{
    public interface ITranslation
    {
        public GenericOutput GetOutput(CodeFile codeFile);
    }
}
