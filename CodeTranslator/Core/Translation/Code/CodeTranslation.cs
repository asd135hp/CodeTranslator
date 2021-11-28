using System;
using System.Threading.Tasks;

using CodeTranslator.Core.Output;
using CodeTranslator.Core.Output.Translated;
using CodeTranslator.Core.Translation.Code.Model;
using CodeTranslator.Model;
using CodeTranslator.Utility;
using CodeTranslator.Utility.Progress;

namespace CodeTranslator.Core.Translation.Code
{
    public class CodeTranslation : ITranslation
    {
        public Language Language { get; set; }

        public ProgressTracker Progress { get; private set; }

        public CodeTranslation() : base()
        {
            Progress = null;
        }

        public IOutput GetOutput(CodeFile codeFile)
        {
            Progress = new ProgressTracker();
            var output = new TranslatedCodeFile(codeFile);

            ulong count = 0;
            foreach (string codeLine in codeFile.CodeLines)
            {
                Progress.AddTask(Task.Run(() =>
                {
                    // somehow translate the line - TODO
                    var translatedCodeLine = "";

                    output.TranslatedCodeLines.AddOrUpdate(
                        count,
                        translatedCodeLine,
                        (lineNumber, oldCodeLine) => translatedCodeLine);
                }));

                count++;
            }

            return output;
        }

        public IObservable<ProgressStatus> GetObservableProgressTracker() => Progress;
    }
}
