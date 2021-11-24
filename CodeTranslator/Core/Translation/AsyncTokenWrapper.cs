using System.Threading;

namespace CodeTranslator.Core.Translation
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

        public void CancelTranslation()
        {
            _source.Cancel();
            _source.Dispose();
        }
    }
}
