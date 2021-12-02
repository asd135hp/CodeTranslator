using System.IO;
using System.Collections.Generic;
using CodeTranslator.Core.Translation.Code;

namespace CodeTranslator.Model
{
    public class Language
    {
        public static LanguageBuilder Builder => new LanguageBuilder();

        internal FileInfo Info { get; set; }
        public bool IsReverseTranslation { get; internal set; }
        public string LanguageName { get; internal set; }
        public Dictionary<string, string> TranslatedKeywords { get; private set; }

        public Language()
        {
            TranslatedKeywords = new Dictionary<string, string>();
        }
    }
}
