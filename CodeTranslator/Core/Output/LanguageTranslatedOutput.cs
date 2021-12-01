using System;
using CodeTranslator.Model.Output;

namespace CodeTranslator.Core.Output
{
    public class LanguageTranslatedOutput : IOutput
    {
        private readonly TranslatedCodeFile _tledFile;

        public LanguageTranslatedOutput()
        {
            _tledFile = new TranslatedCodeFile();
        }

        /// <summary>
        /// A way to update the translation without revealing the component object
        /// </summary>
        /// <param name="index"></param>
        /// <param name="translatedLine"></param>
        /// <param name="updateValueFactory"></param>
        internal void SetTranslation(
            ulong index,
            string translatedLine,
            Func<ulong, string, string> updateValueFactory)
            => _tledFile.TranslatedCodeLines.AddOrUpdate(
                index,
                translatedLine,
                updateValueFactory);

        public string GetCurrent()
            => _tledFile.TranslatedCodeLines[_tledFile.CurrentLineIndex++];

        public bool MoveNext()
        {
            if (!_tledFile.TranslatedCodeLines.ContainsKey(_tledFile.CurrentLineIndex))
                return false;

            _tledFile.CurrentLineIndex++;
            return true;
        }
    }
}
