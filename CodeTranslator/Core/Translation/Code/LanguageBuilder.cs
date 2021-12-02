using System.IO;
using System.Text.Json;
using CodeTranslator.Model;

namespace CodeTranslator.Core.Translation.Code
{
    public class LanguageBuilder
    {
        private string _directory = ".\\translation";
        internal readonly Language _language = new Language()
        {
            Info = null,
            IsReverseTranslation = false
        };

        /// <summary>
        /// Set absolute directory for finding language file.
        /// If this method is not called then the language file must be in a folder named "translation".
        /// The language file must also be in the same execution folder as this program
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public LanguageBuilder SetDirectory(string directory)
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
        public LanguageBuilder SetLanguageFileName(string translationFileName)
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
        public LanguageBuilder SetReverseTranslation(bool enableReverseTranslation)
        {
            _language.IsReverseTranslation = enableReverseTranslation;
            return this;
        }

        /// <summary>
        /// Parse JSON into CodeTranslator.Model.Language class object
        /// </summary>
        /// <param name="jsonDocument"></param>
        private void ParseLanguageFile(JsonDocument jsonDocument)
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
        /// Obligatory method for Builder pattern
        /// </summary>
        /// <returns></returns>
        public Language Build()
        {
            var stream = File.OpenRead(_language.Info.FullName);
            var jd = JsonDocument.Parse(stream, new JsonDocumentOptions()
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            });
            ParseLanguageFile(jd);

            return _language;
        }
    }
}
