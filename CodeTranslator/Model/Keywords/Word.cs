using System.Drawing;
using CodeTranslator.Common;

namespace CodeTranslator.Model.Keywords
{
    public abstract class Word
    {
        private readonly Word _nextWord;

        public abstract WordType WordType { get; }

        /// <summary>
        /// A string representing the word
        /// </summary>
        public abstract string Content { get; }

        /// <summary>
        /// Represent this word's color for linting
        /// </summary>
        public abstract Color Color { get; }

        /// <summary>
        /// Null if this word is at the end of the line.
        /// </summary>
        public Word NextWord => _nextWord;

        protected Word(Word nextWord)
        {
            _nextWord = nextWord;
        }
    }
}
