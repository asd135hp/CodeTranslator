using CodeTranslator.IO;
using CodeTranslator.Core.Translation;
using CodeTranslator.Common;

namespace CodeTranslator.Core.Translator
{
    public class SingleFileTranslator : AbstractTranslator<LocalDirectoryInfo, LocalFileInfo>
    {
        public override TranslatorType Type => TranslatorType.SingleFile;

        public SingleFileTranslator(SingleFileTree rootDirectory, ITranslation translation)
            : base(rootDirectory, translation) { }
    }
}
