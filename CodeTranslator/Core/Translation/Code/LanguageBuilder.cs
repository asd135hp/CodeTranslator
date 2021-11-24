using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CodeTranslator.Core.Translation.Code.Model;

namespace CodeTranslator.Core.Translation.Code
{
    public class LanguageBuilder
    {
        private string _directory = "";
        internal readonly Language _language = new Language()
        {
            Info = null
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
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException($"New language directory ({directory}) must be a local directory");

            _directory = directory;
            return this;
        }

        /// <summary>
        /// Set up language file for builder to start doing works
        /// </summary>
        /// <param name="translationFileName">Only name and extension (.json)</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public LanguageBuilder SetLanguageFile(string translationFileName)
        {
            string fileName = _directory.Length == 0 ?
                $"translation/{translationFileName}" :
                $"{_directory}/{translationFileName}";

            if (!File.Exists(fileName))
                throw new FileNotFoundException($"Could not find local language file: {fileName}");

            _language.Info = new FileInfo(fileName);

            return this;
        }

        /// <summary>
        /// 
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
                if(translatedValue.ValueKind == JsonValueKind.String)
                    _language.TranslatedKeywords[rawKeyword] = translatedValue.GetString();
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
