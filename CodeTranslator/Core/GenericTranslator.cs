using CodeTranslator.Core.Output;
using CodeTranslator.Core.Translation;
using CodeTranslator.Model;
using CodeTranslator.Model.Tree;

namespace CodeTranslator.Core
{
    public abstract class GenericTranslator: ITranslator
    {
        public DirectoryTree RootDirectory { get; internal set; }
        public ITranslation Translation { get; set; }

        public IOutput TranslateFile(CodeFile codeFile)
            => Translation.GetOutput(codeFile);
    }
}
