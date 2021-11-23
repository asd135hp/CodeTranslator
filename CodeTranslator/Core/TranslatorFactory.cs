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
        public ITranslator Create(TranslatorType tt)
            => tt switch
            {
                TranslatorType.Github => new GithubTranslator(),
                TranslatorType.LocalDirectory => new LocalDirectoryTranslator(),
                TranslatorType.SingleFile => new SingleFileTranslator(),
                _ => new NullTranslator()
            };
    }
}
