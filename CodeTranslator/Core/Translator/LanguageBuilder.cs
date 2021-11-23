using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CodeTranslator.Core.Translator.Model;

namespace CodeTranslator.Core.Translator
{
    public class LanguageBuilder
    {
        private string _directory = "";
        private readonly Queue<Task> _tasks = new Queue<Task>();
        internal readonly Language _language = new Language()
        {
            Info = null
        };

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="translationFileName"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public LanguageBuilder SetLanguageFile(string translationFileName)
        {
            string fileName = $"{_directory}/{translationFileName}".TrimStart('/');

            if (!File.Exists(fileName))
                throw new FileNotFoundException($"Could not find local language file: {fileName}");

            _language.Info = new FileInfo(fileName);

            _tasks.Enqueue(Task.Run(() =>
            {
                var jd = JsonDocument.Parse(File.OpenRead(fileName), new JsonDocumentOptions()
                {
                    AllowTrailingCommas = true,
                    CommentHandling = JsonCommentHandling.Skip
                });
                ParseLanguageFile(jd);
            }));

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
        /// 
        /// </summary>
        /// <param name="waitToFinishTasks"></param>
        /// <returns></returns>
        public Language Build(bool waitToFinishTasks = true)
        {
            if (waitToFinishTasks)
                while (_tasks.Count != 0)
                {
                    var task = _tasks.Dequeue();
                    task.Wait();
                    task.Dispose();
                }

            return _language;
        }
    }
}
