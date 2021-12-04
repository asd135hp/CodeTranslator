using System;

using CodeTranslator.Model;
using CodeTranslator.Core.Output;

namespace CodeTranslator.Core.Translation.UML
{
    public sealed class UMLTranslation : ITranslation
    {
        public event EventHandler<TranslationProgressEventArgs> TranslationTracker;

        public UMLTranslation() { }

        public IOutput GetOutput(CodeFile codeFile)
        {
            throw new NotImplementedException();
        }

    }
}
