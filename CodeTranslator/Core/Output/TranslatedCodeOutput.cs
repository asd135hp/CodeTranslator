using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CodeTranslator.Core.Output
{
    public sealed class TranslatedCodeOutput : IOutput
    {
        private static readonly object lockObj = new object { };
        private static readonly string DIRECTORY = $@".\translation\{DateTime.Now}";
        private readonly string _filePath;
        private readonly ILogger _logger;

        public TranslatedCodeOutput(string translatedCode, string fileName, string outputExtension)
        {
            _filePath = $@"{DIRECTORY}\{fileName}.{outputExtension}";
            SaveOutput(translatedCode);
        }

        public TranslatedCodeOutput(
            string translatedCode,
            string fileName,
            string outputExtension,
            ILogger logger) : this(translatedCode, fileName, outputExtension)
        {
            _logger = logger;
        }

        private string SaveOutput(string content)
        {
            var stopWatch = _logger != null ? Stopwatch.StartNew() : null;
            lock (lockObj) File.WriteAllText(_filePath, content);
            _logger?.LogInformation(
                "Successfully saved translation to {Path} in {TimeElapsed}ms",
                _filePath,
                stopWatch.ElapsedMilliseconds
            );
            stopWatch?.Stop();
            return _filePath;
        }

        public string GetContent() => File.ReadAllText(_filePath);

        public string[] GetContentLines() => File.ReadAllLines(_filePath);

        public Task<string> GetContentAsync(CancellationToken cancellationToken = default)
            => File.ReadAllTextAsync(_filePath, cancellationToken);

        public Task<string[]> GetContentLinesAsync(CancellationToken cancellationToken = default)
            => File.ReadAllLinesAsync(_filePath, cancellationToken);
    }
}
