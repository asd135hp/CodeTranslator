using System.Threading.Tasks;

using CodeTranslator.Core.Output;
using CodeTranslator.Core.Output.Translated;
using CodeTranslator.Core.Translation.Code.Model;
using CodeTranslator.Model;
using CodeTranslator.Utility;

namespace CodeTranslator.Core.Translation.Code
{
    public class CodeTranslation : AsyncTokenWrapper, ITranslation
    {
        public Language Language { get; set; }

        public ProgressStatus Progress { get; private set; }

        public GenericOutput GetOutput(CodeFile codeFile)
        {
            Progress = Progress.GetInstance();
            var output = new TranslatedCodeFile(codeFile);

            Task.Run(() =>
            {
                ulong count = 0;
                foreach(string codeLine in codeFile.CodeLines)
                {
                    // somehow translate the line - TODO
                    var translatedCodeLine = "";

                    output.TranslatedCodeLines[count] = translatedCodeLine;

                    // end translation
                    if (_token.IsCancellationRequested) break;
                    count++;
                }

                // dispose the progress management object
                Progress.Dispose();
            }, _token);

            return output;
        }
    }
}
