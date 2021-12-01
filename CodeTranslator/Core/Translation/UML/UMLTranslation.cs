using System;

using CodeTranslator.Model;
using CodeTranslator.Core.Output;
using CodeTranslator.Utility.Progress;

namespace CodeTranslator.Core.Translation.UML
{
    public sealed class UMLTranslation : ITranslation
    {
        public ProgressTracker Progress { get; private set; }

        public UMLTranslation()
        {
            Progress = null;
        }

        public IOutput GetOutput(CodeFile codeFile)
        {
            throw new NotImplementedException();
        }
        public IObservable<ProgressStatus> GetObservableProgressTracker() => Progress;
    }
}
