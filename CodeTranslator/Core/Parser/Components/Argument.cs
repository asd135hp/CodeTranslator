namespace CodeTranslator.Core.Parser.Components
{
    public class Argument
    {
        internal Variable variable;

        public string Modifier { get; set; }
        
        public string Name => variable.Name;

        public string Type => variable.Type;

        public string DefaultValue { get; set; }
    }
}
