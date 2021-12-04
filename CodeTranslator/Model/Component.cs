using System.Reflection;

namespace CodeTranslator.Model
{
    public class Component
    {
        #region Non-Letter characters

        #region Constants

        public const string Add = "+";
        public const string Ampersand = "&";
        public const string Asterisk = "*";
        public const string BackSlash = "\\";
        public const string Caret = "^";
        public const string Colon = ":";
        public const string Comma = ",";
        public const string CloseParenthesis = ")";
        public const string CloseSquareBracket = "]";
        public const string DollarSign = "$";
        public const string DoubleQuote = "\"";
        public const string Dot = ".";
        public const string Equal = "=";
        public const string HashTag = "#";
        public const string Hyphen = "-";
        public const string GreaterThan = ">";
        public const string GreaterThanOrEqual = GreaterThan + Equal;
        public const string LessThan = "<";
        public const string LessThanOrEqual = LessThan + Equal;
        public const string OpenParenthesis = "(";
        public const string OpenSquareBracket = "[";
        public const string Percentage = "%";
        public const string QuestionMark = "?";
        public const string Quote = "'";
        public const string SemiColon = ";";
        public const string Slash = "/";
        public const string Tab = "\t";
        public const string ThickArrow = Equal + GreaterThan;
        public const string ThinArrow = Hyphen + GreaterThan;
        public const string Tide = "~";
        public const string Tick = "`";
        public const string VerticalSlash = "|";
        public const string Underscore = "_";
        public const string WhiteSpace = " ";

        #endregion

        /// <summary>
        /// Known arithmetic signs
        /// </summary>
        public string ArithmeticOps => $"{Add} {Hyphen} {Asterisk} {Slash} {Percentage} {Slash}{Slash} {Caret}";

        /// <summary>
        /// Any type of indentation that a language could have
        /// </summary>
        public string Indent { get; set; }

        /// <summary>
        /// Could be English based ops like "and" instead of && or "not" instead of !
        /// </summary>
        public string LogicalOps { get; set; }

        /// <summary>
        /// Could be English based ops like "and" instead of & or "or" instead of |
        /// <para>
        /// When defining this, be careful to not make logical ops and bitwise ops
        /// become ambiguous
        /// </para>
        /// </summary>
        public string BitwiseOps { get; set; }

        /// <summary>
        /// Normally, the ops would be this set: ==, >, <.
        /// However, it could be modified to accept === too
        /// </summary>
        public string ComparisionOps { get; set; }

        /// <summary>
        /// In low level languages or unsafe blocks, this is necessary.
        /// Must specify Reference too or an exception will be thrown.
        /// <para>Default: Asterisk (*)</para>
        /// </summary>
        public string Pointer { get; set; }

        /// <summary>
        /// In low level languages or unsafe blocks, this is necessary.
        /// Must specify Pointer too or an exception will be thrown.
        /// <para>Default: Ampersand (&)</para>
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Mainly for arrow functions, could be => or ->
        /// <para>Default: ThickArrow (=>)</para>
        /// </summary>
        public string Arrow { get; set; }

        /// <summary>
        /// Normally, this would be a semi-colon. Some languages do not need semi-colons
        /// for end of line indication
        /// <para>Default: SemiColon (;)</para>
        /// </summary>
        public string EndOfLine { get; set; }

        #endregion

        #region

        /// <summary>
        /// In C#, the modifiers are "private|protected|internal|public"
        /// </summary>
        public string AccessModifier { get; set; }

        /// <summary>
        /// In C#, it is called "sealed" while in Java, it is "final" instead
        /// </summary>
        public string InheritancePrevention { get; set; }

        #endregion

        public Component(bool allowDefaultImplementation = true)
        {
            if (allowDefaultImplementation)
            {
                Pointer = Asterisk;
                Reference = Ampersand;
                Arrow = ThickArrow;
                EndOfLine = SemiColon;
                // non-fancy way to initialize a 4 whitespaces indent
                Indent = WhiteSpace + WhiteSpace + WhiteSpace + WhiteSpace;
            }
            else
            {
                foreach(PropertyInfo propInfo in GetType().GetProperties())
                    // all properties are strings so this is fine
                    propInfo.SetValue(this, string.Empty);
            }
        }
    }
}
