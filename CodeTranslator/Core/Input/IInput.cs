using CodeTranslator.Core.Tree;

namespace CodeTranslator.Core.Input
{
    public interface IInput
    {
        GitHubDirectoryTree ToGitHubDirectoryTree();
        LocalDirectoryTree ToLocalDirectoryTree();
        SingleFileTree ToSingleFileDirectoryTree();
    }
}
