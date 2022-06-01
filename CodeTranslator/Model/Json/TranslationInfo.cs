using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace CodeTranslator.Model.Json
{
    public class TranslationInfo
    {
        [JsonPropertyName("name")] public string LanguageName { get; set; }
        [JsonPropertyName("translation")] public Dictionary<string, string> TranslatedKeywords { get; set; }
    }
}
