using System.IO;
using System.Collections.Generic;
using CodeTranslator.Core.Translation.Code;

namespace CodeTranslator.Model
{
    public class Language
    {
        public static LanguageBuilder Builder => new LanguageBuilder();

        internal FileInfo Info { get; set; }
        public string LanguageName { get; internal set; }
        public readonly Dictionary<string, string> TranslatedKeywords;

        public Language()
        {
            TranslatedKeywords = new Dictionary<string, string>();
        }
    }
}
