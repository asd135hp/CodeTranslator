using CodeTranslator.IO;
using CodeTranslator.Core.Translation;
using CodeTranslator.Common;

namespace CodeTranslator.Core.Translator
{
    public sealed class GithubTranslator
        : AbstractTranslator<GithubDirectoryInfo, GithubFileInfo>
    {
        public override TranslatorType Type => TranslatorType.GitHub;

        public GithubTranslator(GithubDirectoryTree directoryTree, ITranslation translation)
            : base(directoryTree, translation)
        {

        }
    }
}
