using System.IO;
using System.Collections.Generic;

namespace CodeTranslator.Model.Output
{
    internal sealed class TranslatedCodeFile
    {
        private const string DIRECTORY = @".\translation";

        /// <summary>
        /// Indicates that the file should not be translated anymore if true
        /// </summary>
        public bool IsTranslatedFromCachedFile { get; private set; }
        
        /// <summary>
        /// Local path to the translation file
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Cached position for future code line reading
        /// </summary>
        public long CurrentLineIndex { get; set; }

        /// <summary>
        /// Dictionary for caching translated words
        /// </summary>
        public Dictionary<long, string> TranslatedCodeLines { get; private set; }

        public TranslatedCodeFile(string fileName, bool isReverseTranslation)
        {
            FilePath = !isReverseTranslation ?
                $@"{DIRECTORY}\{fileName}translation" :
                $@"{DIRECTORY}\{fileName.TrimEnd("translation")}";
            CurrentLineIndex = 0;
            TranslatedCodeLines = new Dictionary<long, string>();

            // create new directory for storing translation file
            if (!Directory.Exists(DIRECTORY))
                Directory.CreateDirectory(DIRECTORY);

            // if the file has already existed, cache the file and deny future changes
            // to this object
            if (IsTranslatedFromCachedFile = File.Exists(FilePath))
            {
                using var stream = File.OpenText(FilePath);
                string line;
                long index = 0L;
                while ((line = stream.ReadLine()) != null)
                {
                    TranslatedCodeLines[index++] = line;
                }
            }
        }
    }
}
