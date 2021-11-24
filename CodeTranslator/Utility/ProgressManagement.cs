using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeTranslator.Utility
{
    public class ProgressManagement : IDisposable
    {
        private readonly CancellationTokenSource _source = null;
        private readonly CancellationToken _token;
        private readonly Task _counter = null;
        private readonly List<Action<ProgressManagement>> _externalActions;

        private static ProgressManagement _singleton = null;
        private List<Task> _currentTasks;

        /// <summary>
        /// After calling Dispose(), this property will always return false
        /// </summary>
        public bool IsProgressCounterFinished => _counter?.IsCompletedSuccessfully ?? false;
        public int TasksCompleted { get; private set; }
        public int TotalTaskCount { get; private set; }
        public IEnumerable<Task> CurrentTasks => _currentTasks;

        public static ProgressManagement GetInstance()
        {
            if(_singleton == null)
            {
                _singleton = new ProgressManagement();
            }
            return _singleton;
        }

        private ProgressManagement()
        {
            _externalActions = new List<Action<ProgressManagement>>();
            _currentTasks = new List<Task>();

            // refreshes counter whenever the main constructor is called
            TasksCompleted = 0;
            TotalTaskCount = 0;

            // start monitoring completed tasks first before the list even popularized
            _source = new CancellationTokenSource();
            _token = _source.Token;
            _counter = Task.Run(async () =>
            {
                while (TasksCompleted != TotalTaskCount && !_token.IsCancellationRequested)
                {
                    await Task.Delay(1000);
                    
                    // count completed tasks first
                    CountCompletedTasks();

                    // then start all registered actions
                    foreach (var action in _externalActions) action.Invoke(this);
                }
            }, _token);
        }

        /// <summary>
        /// Add a single task to start monitoring the progress
        /// </summary>
        /// <param name="newTask"></param>
        public void AddTask(Task newTask)
        {
            _currentTasks.Add(newTask);
            TotalTaskCount = _currentTasks.Count;
        }

        /// <summary>
        /// Add tasks to start monitoring the progress
        /// </summary>
        /// <param name="newTask"></param>
        public void AddTask(params Task[] newTasks)
        {
            _currentTasks.AddRange(newTasks);
            TotalTaskCount = _currentTasks.Count;
        }

        /// <summary>
        /// <para>Add new action to the list of registered actions for progress counter task.</para>
        /// <para>WARNING: The actions pushed to the list will all be simultaneously called
        /// in an interval of 1 second until the progress counter task finished.</para>
        /// </summary>
        /// <param name="action"></param>
        public void AddCustomActionToCounter(Action<ProgressManagement> action)
            => _externalActions.Add(action);

        /// <summary>
        /// Provide a thread-safe operation for counting the number of finished tasks
        /// </summary>
        private void CountCompletedTasks()
        {
            var newList = _currentTasks.SkipWhile((task) => task.IsCompleted).ToList();
            TasksCompleted += _currentTasks.Count - newList.Count;
            _currentTasks = newList;
        }

        public void Dispose()
        {
            // safely dispose old components
            try
            {
                _source?.Cancel();
                _source?.Dispose();
                _counter?.Dispose();
                _currentTasks.Clear();
                _externalActions.Clear();
                _singleton = null;
            }
            catch(Exception e)
            {
                // log here
            }
        }
    }
}
