using CodeTranslator.Core.Translator.Github;
using CodeTranslator.Core.Translator.LocalDirectory;
using CodeTranslator.Core.Translator.SingleFile;
using CodeTranslator.Core.Translator.Null;

namespace CodeTranslator.Core
{
    public enum TranslatorType
    {
        Github,
        LocalDirectory,
        SingleFile,
        None
    }

    public class TranslatorFactory
    {
        public ITranslator Create(TranslatorType tt, string path)
            => tt switch
            {
                TranslatorType.Github => new GithubTranslator(path),
                TranslatorType.LocalDirectory => new LocalDirectoryTranslator(path),
                TranslatorType.SingleFile => new SingleFileTranslator(),
                _ => new NullTranslator()
            };
    }
}
