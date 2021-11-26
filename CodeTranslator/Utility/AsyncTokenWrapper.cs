using System.Threading;

namespace CodeTranslator.Utility
{
    public abstract class AsyncTokenWrapper
    {
        private readonly CancellationTokenSource _source = null;
        protected readonly CancellationToken _token;

        public AsyncTokenWrapper()
        {
            _source = new CancellationTokenSource();
            _token = _source.Token;
        }

        internal void CancelAllTasks()
        {
            _source.Cancel();
            _source.Dispose();
        }
    }
}
