using CodeTranslator.Core.Model;
using CodeTranslator.Core.Output;
using CodeTranslator.Core.Translation;

namespace CodeTranslator.Core
{
    public abstract class GenericTranslator: ITranslator
    {
        public DirectoryTree RootDirectory { get; protected set; }
        public ITranslation Translation { get; set; }

        public GenericOutput TranslateFile(CodeFile codeFile)
            => Translation.GetOutput(codeFile);
    }
}
