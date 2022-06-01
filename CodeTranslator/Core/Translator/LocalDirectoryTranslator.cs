using CodeTranslator.IO;
using CodeTranslator.Core.Parser;
using CodeTranslator.Core.Tree;
using CodeTranslator.Common;

namespace CodeTranslator.Core.Translator
{
    public sealed class LocalDirectoryTranslator
        : AbstractTranslator<LocalDirectoryInfo, LocalFileInfo>
    {
        public override TranslatorType Type => TranslatorType.LocalDirectory;

        public LocalDirectoryTranslator(LocalDirectoryTree directoryTree, IParser parser)
            : base(directoryTree, parser)
        {

        }
    }
}
