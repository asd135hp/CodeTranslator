using CodeTranslator.Model;

namespace CodeTranslator.Core.Output
{
    public abstract class GenericOutput
    {
        public readonly CodeFile CodeFile;

        public GenericOutput(CodeFile codeFile)
        {
            CodeFile = codeFile;
        }
    }
}
