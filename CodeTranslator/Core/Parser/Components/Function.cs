using CodeTranslator.Common;

namespace CodeTranslator.Core.Parser.Components
{
    public abstract class Function
    {
        protected string modifier, type, name;
        protected ArgumentList arguments;

        public string Modifier => modifier;

        public string Type => type;

        public string Name => name;

        public ArgumentList Arguments => arguments;

        public Function(string content)
        {

        }

        public abstract BasicType FunctionType { get; }
    }
}
