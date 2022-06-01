using CodeTranslator.IO;
using CodeTranslator.Core.Parser;
using CodeTranslator.Core.Tree;
using CodeTranslator.Common;

namespace CodeTranslator.Core.Translator
{
    public sealed class GitHubTranslator
        : AbstractTranslator<GitHubDirectoryInfo, GitHubFileInfo>
    {
        public override TranslatorType Type => TranslatorType.GitHub;

        public GitHubTranslator(GitHubDirectoryTree directoryTree, IParser parser)
            : base(directoryTree, parser)
        {

        }
    }
}
