using System;
using CodeTranslator.Core.Output;
using CodeTranslator.Model;

namespace CodeTranslator.Core.Translation
{
    public interface ITranslation
    {
        public IOutput GetOutput(CodeFile codeFile);
        public IObservable<ProgressStatus> GetObservableProgressTracker(); 
    }
}
