using CodeTranslator.Core.Translator;

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
