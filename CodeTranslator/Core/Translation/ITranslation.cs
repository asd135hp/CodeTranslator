using CodeTranslator.Core.Output;
using CodeTranslator.Model;

namespace CodeTranslator.Core.Translation
{
    public interface ITranslation
    {
        public GenericOutput GetOutput(CodeFile codeFile);
    }
}
