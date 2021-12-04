using System;
using System.Linq;

using CodeTranslator.Core.Output;
using CodeTranslator.Model;
using CodeTranslator.Core.Parser;

namespace CodeTranslator.Core.Translation.Code
{
    public sealed class CodeTranslation : ITranslation
    {
        public Model.Translation Translation { get; set; }

        public event EventHandler<TranslationProgressEventArgs> TranslationTracker;

        public CodeTranslation() { }

        public IOutput GetOutput(CodeFile codeFile)
        {
            var output = new LanguageTranslatedOutput(
                codeFile.Info.Name,
                Translation.Language.IsReverseTranslation);
            var parser = new CodeParser()
            {
                Rules = Translation?.Rules,
                Settings = Translation?.Settings
            };

            long count = 0L;
            var args = new TranslationProgressEventArgs(0, codeFile.CodeLines.Count());
            foreach (string codeLine in codeFile.CodeLines)
            {
                // somehow translate the line - TODO
                var translatedCodeLine = Translate(codeLine);
                output.StoreTranslation(count, translatedCodeLine);
                args.Update(count++);
                TranslationTracker?.Invoke(this, args);
            }

            return output;
        }

        private string Translate(string input)
            => _keywordPattern.Replace(input, match =>
            {
                string value = match.Value;
                var lang = Translation.Language;
                return lang.TranslatedKeywords.ContainsKey(value) ?
                    lang.TranslatedKeywords[value] : value;
            });
    }
}
