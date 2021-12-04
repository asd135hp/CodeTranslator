using System.Collections.Generic;
using CodeTranslator.Rules;

namespace CodeTranslator.Model
{
    public class Translation
    {
        public Language Language { get; set; }
        public IEnumerable<IRule> Rules { get; set; }
        public CodeTranslationSettings Settings { get; set; }
    }
}
