using System;
using System.IO;

using CodeTranslator.Model.Output;
using Microsoft.Extensions.Logging;

namespace CodeTranslator.Core.Output
{
    public class LanguageTranslatedOutput : IOutput
    {
        private static readonly object lockObj = new object { };
        private readonly TranslatedCodeFile _tledFile;

        internal ILogger Logger { get; set; }

        public LanguageTranslatedOutput(string fileName, bool isReverseTranslation)
        {
            _tledFile = new TranslatedCodeFile(fileName, isReverseTranslation);
        }

        /// <summary>
        /// A way to update the translation without revealing the component object
        /// </summary>
        /// <param name="index"></param>
        /// <param name="translatedLine"></param>
        internal void StoreTranslation(long index, string translatedLine)
        {
            if(_tledFile.IsTranslatedFromCachedFile)
                _tledFile.TranslatedCodeLines[index] = translatedLine;
        }

        public string GetCurrent()
            => _tledFile.TranslatedCodeLines[_tledFile.CurrentLineIndex++];

        public bool MoveNext()
        {
            if (!_tledFile.TranslatedCodeLines.ContainsKey(_tledFile.CurrentLineIndex))
                return false;

            _tledFile.CurrentLineIndex++;
            return true;
        }

        public string SaveOutput()
        {
            var index = 0L;
            var dict = _tledFile.TranslatedCodeLines;

            lock (lockObj)
            {
                File.WriteAllText(_tledFile.FilePath, "");

                using var file = File.AppendText(_tledFile.FilePath);
                while (dict.ContainsKey(index))
                {
                    file.WriteLine(_tledFile.TranslatedCodeLines[index++]);
                }
            }

            Logger?.LogInformation(
                $"Successfully saved translation to {_tledFile.FilePath} with {index} lines");

            return _tledFile.FilePath;
        }
    }
}
