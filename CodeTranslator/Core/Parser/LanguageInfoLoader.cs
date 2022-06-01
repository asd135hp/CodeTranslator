using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using CodeTranslator.Model.Json;
using CodeTranslator.Core.Parser.Rules;

namespace CodeTranslator.Core.Parser
{
    public abstract class LanguageInfoLoader
    {
        protected string fileExtension;
        protected IDictionary<string, string> customComponents;
        protected IDictionary<string, IRule> rules;
        protected ParserSettings parserSettings;

        public void LoadLanguageInfo(string path)
        {
            if (path.Split('.').Last() != "json")
                throw new ArgumentException("File type must be a JSON file");

            if (!File.Exists(path))
                throw new FileNotFoundException("Could not find and load JSON file");

            LoadLanguageInfo(JsonSerializer.Deserialize<ParserLanguageInfo>(
                File.ReadAllText(path),
                new JsonSerializerOptions()
                {
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                }));
        }

        public void LoadLanguageInfo(ParserLanguageInfo languageInfo)
        {
            customComponents = languageInfo.Components;
            parserSettings = languageInfo.Settings;
            fileExtension = languageInfo.FileExtension;
            rules = new Dictionary<string, IRule>();

            foreach (var pair in languageInfo.Rules)
                rules[pair.Key] = new CustomRule(pair.Value);
        }
    }
}
