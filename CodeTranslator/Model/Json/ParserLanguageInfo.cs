using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeTranslator.Model.Json
{
    public class ParserLanguageInfo
    {
        [JsonPropertyName("fileExtension")] public string FileExtension { get; set; }
        [JsonPropertyName("components")] public IDictionary<string, string> Components { get; set; }
        [JsonPropertyName("rules")] public IDictionary<string, string> Rules { get; set; }
        [JsonPropertyName("settings")] public ParserSettings Settings { get; set; }
    }
}
