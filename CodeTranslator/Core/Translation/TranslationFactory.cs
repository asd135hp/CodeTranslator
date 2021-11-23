using CodeTranslator.Core.Translation.Code;
using CodeTranslator.Core.Translation.UML;

namespace CodeTranslator.Core.Translation
{
    public enum TranslationType
    {
        Code,
        UML
    }

    public class TranslationFactory
    {
        public static ITranslation Create(TranslationType tt)
            => tt switch
            {
                TranslationType.Code => new CodeTranslation(),
                TranslationType.UML => new UMLTranslation(),
                _ => null
            };
    }
}
