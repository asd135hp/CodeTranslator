using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using CodeTranslator.Model;
using CodeTranslator.Rules;

namespace CodeTranslator.Core.Translation.Code
{
    public class TranslationBuilder
    {
        private string _directory = ".\\translation";
        private readonly Language _language = new Language()
        {
            Info = null,
            IsReverseTranslation = false
        };
        private readonly List<IRule> _rules = new List<IRule>();


        /// <summary>
        /// Set absolute directory for finding language file.
        /// If this method is not called then the language file must be in a folder named "translation".
        /// The language file must also be in the same execution folder as this program
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public TranslationBuilder SetDirectory(string directory)
        {
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                throw new DirectoryNotFoundException(
                    $"New language directory ({directory}) must be a local directory");

            _directory = directory;
            return this;
        }

        /// <summary>
        /// Set up language file for builder to start doing works
        /// </summary>
        /// <param name="translationFileName">Only name and extension (.json)</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public TranslationBuilder SetTranslationFileName(string translationFileName)
        {
            string filePath = File.Exists(translationFileName) ?
                translationFileName : $@"{_directory}\{translationFileName}";

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Could not find local language file: {filePath}");

            _language.Info = new FileInfo(filePath);

            return this;
        }

        /// <summary>
        /// Enable reverse translation, making it possible to write code in another language
        /// and translate it to the corresponding code file
        /// </summary>
        /// <param name="enableReverseTranslation"></param>
        /// <returns></returns>
        public TranslationBuilder SetReverseTranslation(bool enableReverseTranslation)
        {
            _language.IsReverseTranslation = enableReverseTranslation;
            return this;
        }

        /// <summary>
        /// Parse JSON into CodeTranslator.Model.Language class object
        /// </summary>
        /// <param name="jsonDocument"></param>
        private void ParseLanguage(JsonDocument jsonDocument)
        {
            var jsonRoot = jsonDocument.RootElement;
            
            // get language name (must be a string)
            if (!jsonRoot.TryGetProperty("name", out var languageName) ||
                languageName.ValueKind != JsonValueKind.String)
                throw new JsonException("Wrong language JSON file format!");

            _language.LanguageName = languageName.GetString();

            // get translated keywords (must be a json object)
            if (!jsonRoot.TryGetProperty("translation", out var translationJson) ||
                translationJson.ValueKind != JsonValueKind.Object)
                throw new JsonException("Wrong language JSON file format!");

            foreach (var translatedKeywords in translationJson.EnumerateObject())
            {
                var rawKeyword = translatedKeywords.Name;
                var translatedValue = translatedKeywords.Value;
                if (translatedValue.ValueKind == JsonValueKind.String)
                {
                    var translatedKeyword = translatedValue.GetString();
                    if (_language.IsReverseTranslation)
                        _language.TranslatedKeywords[translatedKeyword] = rawKeyword;
                    else
                        _language.TranslatedKeywords[rawKeyword] = translatedKeyword;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonDocument"></param>
        private IEnumerable<IRule> GetRuleSets(JsonDocument jsonDocument)
        {
            var result = new List<IRule>();
            var jsonRoot = jsonDocument.RootElement;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonDocument"></param>
        private CodeTranslationSettings GetSettings(JsonDocument jsonDocument)
        {
            var settings = new CodeTranslationSettings();
            var jsonRoot = jsonDocument.RootElement;


            return settings;
        }

        /// <summary>
        /// Obligatory method for Builder pattern
        /// </summary>
        /// <returns></returns>
        public Model.Translation Build()
        {
            var stream = File.OpenRead(_language.Info.FullName);
            var jd = JsonDocument.Parse(stream, new JsonDocumentOptions()
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            });

            ParseLanguage(jd);

            return new Model.Translation()
            {
                Language = _language,
                Rules = GetRuleSets(jd),
                Settings = GetSettings(jd)
            };
        }
    }
}
