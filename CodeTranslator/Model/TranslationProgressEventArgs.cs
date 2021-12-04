using System;

namespace CodeTranslator.Model
{
    public class TranslationProgressEventArgs : EventArgs
    {
        private long _lineNumber;
        private readonly long _totalLines;

        public long LineNumber => _lineNumber;
        public long TotalLines => _totalLines;

        public TranslationProgressEventArgs(long lineNumber, long totalLines)
        {
            _lineNumber = lineNumber;
            _totalLines = totalLines;
        }

        internal void Update(long lineNumber) => _lineNumber = lineNumber;
    }
}
