using System.Collections.Concurrent;

namespace CodeTranslator.Model.Output
{
    internal sealed class TranslatedCodeFile
    {
        public ulong CurrentLineIndex { get; set; }
        public readonly ConcurrentDictionary<ulong, string> TranslatedCodeLines;

        public TranslatedCodeFile()
        {
            CurrentLineIndex = 0;
            TranslatedCodeLines = new ConcurrentDictionary<ulong, string>();
        }
    }
}
