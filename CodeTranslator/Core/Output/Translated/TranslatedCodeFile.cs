using System.Collections.Concurrent;
using CodeTranslator.Model;

namespace CodeTranslator.Core.Output.Translated
{
    public sealed class TranslatedCodeFile : GenericOutput
    {
        private ulong _currentLineIndex;
        internal readonly ConcurrentDictionary<ulong, string> TranslatedCodeLines;

        public TranslatedCodeFile(CodeFile codeFile) : base(codeFile)
        {
            _currentLineIndex = 0;
            TranslatedCodeLines = new ConcurrentDictionary<ulong, string>();
        }

        /// <summary>
        /// Set current cursor to the specified line number, starting from 1
        /// </summary>
        /// <param name="lineNumber">If 0 is provided, the current line number will be 1</param>
        public void SetLineNumber(ulong lineNumber)
            => _currentLineIndex = lineNumber == 0 ? 0 : lineNumber - 1;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string NextCodeLine() => TranslatedCodeLines[_currentLineIndex++];
    }
}
