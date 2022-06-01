using CodeTranslator.Core.Translator;

namespace CodeTranslator.Core.Input
{
    public interface IInput
    {
        string RootPath { get; set; }

        GitHubTranslator GetGitHubTranslator(
            string productName,
            string accessToken,
            string commitReference,
            string branch = "");

        LocalDirectoryTranslator GetLocalDirectoryTranslator();

        SingleFileTranslator GetSingleFileTranslator();

        ZipFileTranslator GetZipFileTranslator();
    }
}
