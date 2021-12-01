using CodeTranslator.IO;
using CodeTranslator.Core.Translation;
using CodeTranslator.Common;

namespace CodeTranslator.Core.Translator
{
    public sealed class LocalDirectoryTranslator
        : AbstractTranslator<LocalDirectoryInfo, LocalFileInfo>
    {
        public override TranslatorType Type => TranslatorType.LocalDirectory;

        public LocalDirectoryTranslator(
            LocalDirectoryTree directoryTree,
            ITranslation translation)
            : base(directoryTree, translation)
        {

        }
    }
}
