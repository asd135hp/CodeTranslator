using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using CodeTranslator.Core.Output;
using CodeTranslator.Model;
using CodeTranslator.Utility.Progress;

namespace CodeTranslator.Core.Translation.Code
{
    public class CodeTranslation : ITranslation
    {
        private ProgressTracker _tracker;
        private static readonly Regex _keywordPattern
            = new Regex(@"(?<=[(\s])(?:[a-zA-Z]+)(?=[,\s])");

        public Language Language { get; set; }

        public ProgressTracker Progress => _tracker;

        public CodeTranslation() : base()
        {
            _tracker = null;
        }

        public IOutput GetOutput(CodeFile codeFile)
        {
            _tracker = new ProgressTracker();
            var output = new LanguageTranslatedOutput(
                codeFile.Info.Name,
                Language.IsReverseTranslation);

            ulong count = 0;
            foreach (string codeLine in codeFile.CodeLines)
            {
                Progress.AddTask(Task.Run(() =>
                {
                    // somehow translate the line - TODO
                    var translatedCodeLine = Translate(codeLine);
                    output.StoreTranslation(
                        count,
                        translatedCodeLine,
                        (lineNumber, oldCodeLine) => translatedCodeLine);
                }));

                count++;
            }

            return output;
        }

        public IObservable<ProgressStatus> GetObservableProgressTracker() => Progress;

        private string Translate(string input)
            => _keywordPattern.Replace(input, match =>
            {
                string value = match.Value;
                return Language.TranslatedKeywords.ContainsKey(value) ?
                    Language.TranslatedKeywords[value] : value;
            });
    }
}
