using CodeTranslator.IO;
using CodeTranslator.Core.Translation;
using CodeTranslator.Core.Tree;
using CodeTranslator.Common;

namespace CodeTranslator.Core.Translator
{
    public sealed class GitHubTranslator
        : AbstractTranslator<GitHubDirectoryInfo, GitHubFileInfo>
    {
        public override TranslatorType Type => TranslatorType.GitHub;

        public GitHubTranslator(GitHubDirectoryTree directoryTree, ITranslation translation)
            : base(directoryTree, translation)
        {

        }
    }
}
