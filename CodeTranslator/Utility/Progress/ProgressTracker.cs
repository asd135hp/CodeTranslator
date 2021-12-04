using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeTranslator.Model;

namespace CodeTranslator.Utility.Progress
{
    public class ProgressTracker: AsyncTokenWrapper, IObservable<ProgressStatus>
    {
        // event listener
        private readonly Task _counter = null;
        private readonly List<IObserver<ProgressStatus>> _observers;

        private ProgressStatus _progressStatus;
        private List<Task> _currentTasks;

        public bool IsFinished { get; private set; }

        public int TrackingInterval { get; set; }

        public event EventHandler<ProgressStatus> ProgressStatusChanged;

        public ProgressTracker(int trackingInterval = 1000) : base()
        {
            TrackingInterval = trackingInterval;
            IsFinished = false;

            _progressStatus = new ProgressStatus();
            _currentTasks = new List<Task>();
            _observers = new List<IObserver<ProgressStatus>>();

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

                        // inform all observers about the change
                        foreach (var observer in _observers) observer.OnNext(_progressStatus);

                        // send out information for events
                        ProgressStatusChanged?.Invoke(this, _progressStatus);

                        continue;
                    }

                    // inform all observers about the cancellation
                    foreach (var observer in _observers)
                        observer.OnError(new Exception("Progress observation ended suddenly"));

                    _progressStatus.IsCancelled = true;
                    ProgressStatusChanged?.Invoke(this, _progressStatus);

                    break;
                }

                if (!_token.IsCancellationRequested)
                {
                    foreach (var observer in _observers)
                        observer.OnCompleted();

                    _progressStatus.IsCompleted = true;
                    ProgressStatusChanged?.Invoke(this, _progressStatus);
                }

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

            return new ObserversUnsubscriber<ProgressStatus>(_observers, observer);
        }

        #endregion
    }
}
