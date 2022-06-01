using System;
using CodeTranslator.Core.Translator;

namespace CodeTranslator.Core.Input
{
    public class CommandLineInput : IInput
    {
        public GitHubTranslator GetGitHubTranslator()
        {
            throw new NotImplementedException();
        }

        public LocalDirectoryTranslator GetLocalDirectoryTranslator()
        {
            throw new NotImplementedException();
        }

        public SingleFileTranslator GetSingleFileTranslator()
        {
            throw new NotImplementedException();
        }

        public ZipFileTranslator GetZipFileTranslator()
        {
            throw new NotImplementedException();
        }
    }
}
