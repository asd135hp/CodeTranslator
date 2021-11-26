using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTranslator.Utility.Progress
{
    public class ProgressTracker: AsyncTokenWrapper, IObservable<ProgressStatus>
    {
        private readonly Task _counter = null;
        private readonly List<Action<ProgressTracker>> _externalActions;
        private readonly List<IObserver<ProgressStatus>> _observers;

        private ProgressStatus _progressStatus;
        private List<Task> _currentTasks;

        public bool IsFinished { get; private set; }

        public int TrackingInterval { get; set; }

        public ProgressTracker(int trackingInterval = 1000) : base()
        {
            TrackingInterval = trackingInterval;
            IsFinished = false;

            _progressStatus = new ProgressStatus();
            _externalActions = new List<Action<ProgressTracker>>();
            _currentTasks = new List<Task>();

            // start monitoring completed tasks first before the list even popularized
            _counter = Task.Run(async () =>
            {
                while (_progressStatus.Percentage < 1.0)
                {
                    if (!_token.IsCancellationRequested)
                    {
                        // setting up the interval for this tracking loop
                        await Task.Delay(TrackingInterval);

                        if(_progressStatus.TotalTaskCount != _currentTasks.Count)
                            _progressStatus.TotalTaskCount = _currentTasks.Count;

                        // count completed tasks first
                        CountCompletedTasks();

                        // then start all registered actions
                        foreach (var action in _externalActions) action.Invoke(this);

                        // inform all observers about the change
                        foreach (var observer in _observers) observer.OnNext(_progressStatus);

                        continue;
                    }

                    // inform all observers about the cancellation
                    foreach (var observer in _observers)
                        observer.OnError(new Exception("Progress observation ended suddenly"));

                    break;
                }

                if (!_token.IsCancellationRequested)
                    foreach (var observer in _observers)
                        observer.OnCompleted();

                CleanUp();
            }, _token);
        }

        #region Main Implementation

        /// <summary>
        /// Add a single task to start monitoring the progress
        /// </summary>
        /// <param name="newTask"></param>
        public void AddTask(Task newTask)
        {
            if (!_currentTasks.Contains(newTask))
                _currentTasks.Add(newTask);
        }

        /// <summary>
        /// Add tasks to start monitoring the progress
        /// </summary>
        /// <param name="newTask"></param>
        public void AddTask(params Task[] newTasks)
        {
            foreach(var task in newTasks) AddTask(task);
        }

        /// <summary>
        /// <para>Add new action to the list of registered actions for progress counter task.</para>
        /// <para>WARNING: The actions pushed to the list will all be simultaneously called
        /// in an interval of 1 second until the progress counter task finished.</para>
        /// </summary>
        /// <param name="action"></param>
        public void AddCustomActionToCounter(Action<ProgressTracker> action)
        {
            if (!_externalActions.Contains(action))
                _externalActions.Add(action);
        }

        /// <summary>
        /// Provide a thread-safe operation for counting the number of finished tasks
        /// </summary>
        private void CountCompletedTasks()
        {
            var newList = _currentTasks.SkipWhile((task) => task.IsCompleted).ToList();
            _progressStatus.TasksCompleted += _currentTasks.Count - newList.Count;
            _currentTasks = newList;
        }

        /// <summary>
        /// Clean up everything when the main task is finished
        /// </summary>
        private void CleanUp()
        {
            // safely dispose old components
            try
            {
                CancelAllTasks();
                _counter?.Dispose();
                _currentTasks.Clear();
                _externalActions.Clear();
                _observers.Clear();
            }
            catch (Exception e)
            {
                // log here
            }
            finally
            {
                IsFinished = true;
            }
        }

        #endregion

        #region IObservable Implementation

        IDisposable IObservable<ProgressStatus>.Subscribe(IObserver<ProgressStatus> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
                observer.OnNext(_progressStatus);
            }

            return new Unsubscriber(_observers, observer);
        }

        // until a new solution found
        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<ProgressStatus>> _observers;
            private readonly IObserver<ProgressStatus> _observer;

            public Unsubscriber(
                List<IObserver<ProgressStatus>> observers,
                IObserver<ProgressStatus> observer)
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

        #endregion
    }
}
