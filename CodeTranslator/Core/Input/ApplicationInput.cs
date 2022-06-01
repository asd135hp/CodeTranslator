using CodeTranslator.Core.Tree;
using CodeTranslator.Core.Translator;
using CodeTranslator.Core.Parser;

namespace CodeTranslator.Core.Input
{
    public class ApplicationInput : IInput
    {
        public string RootPath { get; set; }
        public IParser Parser { get; set; }

        public GitHubTranslator GetGitHubTranslator(
            string productName,
            string accessToken,
            string commitReference,
            string branch = "")
        {
            var tree = new GitHubDirectoryTree(productName, accessToken, RootPath, commitReference, branch);
            return new GitHubTranslator(tree, Parser);
        }

        public LocalDirectoryTranslator GetLocalDirectoryTranslator()
        {
            var tree = new LocalDirectoryTree(RootPath);
            return new LocalDirectoryTranslator(tree, Parser);
        }

        public SingleFileTranslator GetSingleFileTranslator()
        {
            var tree = new SingleFileTree(RootPath);
            return new SingleFileTranslator(tree, Parser);
        }

        public ZipFileTranslator GetZipFileTranslator()
            => new ZipFileTranslator(RootPath, Parser);
    }
}
