using CodeTranslator.Core.Translator.Model;

namespace CodeTranslator.Core
{
    public interface ITranslator
    {
        public DirectoryTree Directory { get; set; }
        public Language Language { get; set; }
    }
}
