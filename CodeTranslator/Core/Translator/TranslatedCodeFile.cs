using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CodeTranslator.Core.Translation;
using CodeTranslator.Core.Translator.Model;

namespace CodeTranslator.Core.Translator
{
    public class TranslatedCodeFile : CodeFile
    {
        internal static CancellationTokenSource CancellationTokenSource
            = new CancellationTokenSource();
        private static CancellationToken token = CancellationTokenSource.Token;

        internal static void CancelAllCurrentTasks(bool initializeNewTokenSource = true)
        {
            try
            {
                CancellationTokenSource.Cancel();
                CancellationTokenSource.Dispose();
            }
            finally
            {
                if (initializeNewTokenSource)
                {
                    CancellationTokenSource = new CancellationTokenSource();
                    token = CancellationTokenSource.Token;
                }
            }
        }

        public Task TranslationTask { get; private set; }

        public ITranslation Translation { get; internal set; }

        public TranslatedCodeFile()
        {
            TranslationTask = Task.Run(() =>
            {
                TranslateCodeLines();
                TranslationTask.Dispose();
                TranslationTask = null;
            }, token);
        }

        private void TranslateCodeLines()
        {
            var codeStream = Info.OpenText();
            var codeLines = new List<string>();

            while (true)
            {
                var codeLine = codeStream.ReadLine();
                if (codeLine == null) break;

                // translate each code line here
            }
            

            CodeLines = codeLines;
            LineCount = (uint)codeLines.Count;
        }
    }
}
