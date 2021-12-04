using System.Drawing;
using CodeTranslator.Common;

namespace CodeTranslator.Model.Keywords
{
    public abstract class Keyword : Word
    {
        protected Keyword(Word nextWord) : base(nextWord)
        {
        }

        public override WordType WordType => WordType.Keyword;

        public override Color Color => Color.Blue;

        public abstract KeywordType KeywordType { get; }
    }
}
