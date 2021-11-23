using CodeTranslator.Core.Translator.Model;
using CodeTranslator.Core.Translation.Code.Model;

namespace CodeTranslator.Core
{
    public interface ITranslator
    {
        public DirectoryTree Directory { get; set; }
        public Language Language { get; set; }
    }
}
