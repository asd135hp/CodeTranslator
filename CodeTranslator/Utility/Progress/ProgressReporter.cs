using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTranslator.Utility.Progress
{
    public sealed class ProgressReporter : IObserver<ProgressStatus>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(ProgressStatus value)
        {
            throw new NotImplementedException();
        }
    }
}
