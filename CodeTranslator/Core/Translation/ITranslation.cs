using System;
using CodeTranslator.Core.Output;
using CodeTranslator.Model;
using CodeTranslator.Utility;

namespace CodeTranslator.Core.Translation
{
    public interface ITranslation
    {
        public IOutput GetOutput(CodeFile codeFile);
        public IObservable<ProgressStatus> GetObservableProgressTracker(); 
    }
}
