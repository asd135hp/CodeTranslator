using CodeTranslator.IO;
using CodeTranslator.Core.Parser;
using CodeTranslator.Core.Tree;
using CodeTranslator.Common;

namespace CodeTranslator.Core.Translator
{
    public class SingleFileTranslator : AbstractTranslator<LocalDirectoryInfo, LocalFileInfo>
    {
        public override TranslatorType Type => TranslatorType.SingleFile;

        public SingleFileTranslator(SingleFileTree rootDirectory, IParser parser)
            : base(rootDirectory, parser) { }
    }
}
