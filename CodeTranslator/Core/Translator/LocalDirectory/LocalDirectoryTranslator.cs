using System.IO;

namespace CodeTranslator.Core.Translator.LocalDirectory
{
    public class LocalDirectoryTranslator : GenericTranslator
    {
        public LocalDirectoryTranslator(string rootDirectoryPath)
        {
            if (!Directory.Exists(rootDirectoryPath))
                throw new DirectoryNotFoundException("Provided directory must be a local path in your machine");

            RootDirectory = new LocalDirectoryTree(rootDirectoryPath);
        }
    }
}
