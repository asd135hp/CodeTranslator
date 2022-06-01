namespace CodeTranslator.Core.Parser.Components
{
    public class Component
    {
        public string Value { get; private set; }

        public Component(string value)
        {
            Value = value;
        }
    }
}
