using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeTranslator.Core.Translator.Model
{
    internal class Language: CustomFileInfo
    {
        internal string FilePath { get; set; }
        public string LanguageName { get; internal set; }
        public readonly Dictionary<string, string> TranslatedKeywords;

        public Language()
        {
            TranslatedKeywords = new Dictionary<string, string>();
        }

        public class Builder
        {
            private readonly Queue<Task> _tasks = new Queue<Task>();
            internal readonly Language _language = new Language()
            {
                FilePath = ""
            };

            /// <summary>
            /// 
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            /// <exception cref="DirectoryNotFoundException"></exception>
            public Builder SetLanguageFilePath(string path)
            {
                if (!Directory.Exists(path))
                    throw new DirectoryNotFoundException("New language path must be a local directory");

                _language.FilePath = path;
                return this;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="translationFileName"></param>
            /// <returns></returns>
            /// <exception cref="FileNotFoundException"></exception>
            public Builder SetLanguageFile(string translationFileName)
            {
                string fileName = $"{_language.FilePath}/{translationFileName}".TrimStart('/');

                if (!File.Exists(fileName))
                    throw new FileNotFoundException("New language file must be a local file");

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

            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public Builder Wait()
            {
                while(_tasks.Count != 0)
                {
                    var task = _tasks.Dequeue();
                    task.Wait();
                    task.Dispose();
                }
                return this;
            }

            public Language Build() => _language;
        }
    }
}
