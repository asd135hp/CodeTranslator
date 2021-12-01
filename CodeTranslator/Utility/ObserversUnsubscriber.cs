using System;
using System.Collections.Generic;

namespace CodeTranslator.Utility
{
    internal class ObserversUnsubscriber<T> : IDisposable
    {
        private readonly List<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;

        public ObserversUnsubscriber(
            List<IObserver<T>> observers,
            IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}
