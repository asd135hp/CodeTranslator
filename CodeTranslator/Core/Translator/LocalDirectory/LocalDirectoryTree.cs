using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CodeTranslator.Core.Translator.Model;

namespace CodeTranslator.Core.Translator.LocalDirectory
{
    public class LocalDirectoryTree : DirectoryTree
    {
        private static CancellationTokenSource _source = null;
        private static CancellationToken _token;

        private static List<Task> _codeTranslationTasks;
        private static Task _counter = null;

        public bool IsTaskCounterFinished => _counter.IsCompleted;
        internal static int TasksCompleted = 0;
        internal static int TotalTaskCount { get; private set; }

        private readonly int _depth;
        private const int MAX_DEPTH = 255;

        public LocalDirectoryTree(string rootDirectory) : base(rootDirectory)
        {
            // refreshes task list because this constructor should only be called once
            // and on each recall, all data must be reset to its initial state
            TranslatedCodeFile.CancelAllCurrentTasks();
            _codeTranslationTasks = new List<Task>();

            // refreshes counter whenever the main constructor is called
            TotalTaskCount = 0;
            Task.Run(() => TotalTaskCount = GetTotalFilesCount(rootDirectory));

            // start monitoring completed tasks first before the list even popularized
            _source?.Cancel();
            _counter?.Dispose();

            _source = new CancellationTokenSource();
            _token = _source.Token;
            _counter = Task.Run(async () =>
            {
                while(TasksCompleted != TotalTaskCount && !_token.IsCancellationRequested)
                {
                    await Task.Delay(1000);
                    CountCompletedTasks();
                }
            }, _token);

            // normal stuffs
            _depth = 0;
            PopulateFilesAndDirectories();
        }

        /// <summary>
        /// Hidden constructor for generating child directories objects in the tree recursively
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <param name="parentDirectory"></param>
        /// <param name="depth">
        /// Preventing the program from stepping too deep into sub-directories
        /// </param>
        private LocalDirectoryTree(string rootDirectory, DirectoryTree parentDirectory, int depth)
            : base(rootDirectory, parentDirectory)
        {
            _depth = depth;
            PopulateFilesAndDirectories();
        }

        protected override void PopulateFilesAndDirectories()
        {
            // no more than 255 sub-folders to be registered in the tree
            if (_depth >= MAX_DEPTH) return;

            var childDirs = new List<DirectoryTree>();
            var translatedCodeFiles = new List<TranslatedCodeFile>();
            var currentDir = new DirectoryInfo(DirectoryName);

            // push translated code files and their corresponding task references
            foreach (var codeFile in currentDir.EnumerateFiles())
            {
                var translatedCodeFile = new TranslatedCodeFile()
                {
                    Info = codeFile,
                    ParentDirectory = this
                };

                translatedCodeFiles.Add(translatedCodeFile);
                _codeTranslationTasks.Add(translatedCodeFile.TranslationTask);
            }

            // add child directories to the enumerator
            foreach (var childDir in currentDir.EnumerateDirectories())
                childDirs.Add(new LocalDirectoryTree(childDir.FullName, this, _depth + 1));

            ChildDirectories = childDirs;
            TranslatedCodeFiles = translatedCodeFiles;
        }

        /// <summary>
        /// Gets total number of files that we are dealing with for a given directory
        /// </summary>
        /// <param name="currentDir"></param>
        /// <param name="depth">
        /// Preventing the program from stepping too deep into sub-directories
        /// </param>
        /// <returns></returns>
        private int GetTotalFilesCount(string currentDir, int depth = 0)
        {
            if (depth >= MAX_DEPTH) return 0;

            var filesCount = 0;
            var currentDirInfo = new DirectoryInfo(currentDir);

            foreach(var childDir in currentDirInfo.EnumerateDirectories())
                filesCount += GetTotalFilesCount(childDir.FullName, depth + 1);

            return filesCount + currentDirInfo.GetFiles().Length;
        }

        /// <summary>
        /// Provide a thread-safe operation for counting the number of finished tasks
        /// </summary>
        private void CountCompletedTasks()
        {
            var newList = _codeTranslationTasks.SkipWhile((task) => task.IsCompleted).ToList();
            TasksCompleted += _codeTranslationTasks.Count - newList.Count;
            _codeTranslationTasks = newList;
        }
    }
}
