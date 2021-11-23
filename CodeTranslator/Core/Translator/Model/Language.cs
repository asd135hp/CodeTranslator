using System.IO;
using System.Collections.Generic;

namespace CodeTranslator.Core.Translator.Model
{
    public class Language
    {
        internal FileInfo Info { get; set; }
        public string LanguageName { get; internal set; }
        public readonly Dictionary<string, string> TranslatedKeywords;

        public Language()
        {
            TranslatedKeywords = new Dictionary<string, string>();
        }

        public static LanguageBuilder Builder() => new LanguageBuilder();
    }
}
