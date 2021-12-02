using System.IO;
using System.Collections.Concurrent;

namespace CodeTranslator.Model.Output
{
    internal sealed class TranslatedCodeFile
    {
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
        public ulong CurrentLineIndex { get; set; }

        /// <summary>
        /// Dictionary for caching translated words
        /// </summary>
        public ConcurrentDictionary<ulong, string> TranslatedCodeLines { get; private set; }

        public TranslatedCodeFile(string fileName, bool isReverseTranslation)
        {
            FilePath = !isReverseTranslation ?
                $@".\translation\{fileName}translation" :
                $@".\translation\{fileName.TrimEnd("translation")}";
            CurrentLineIndex = 0;
            TranslatedCodeLines = new ConcurrentDictionary<ulong, string>();

            // create new directory for storing translation file
            if (!Directory.Exists(@".\translation"))
                Directory.CreateDirectory(@".\translation");

            // if the file has already existed, cache the file and deny future changes
            // to this object
            if (IsTranslatedFromCachedFile = File.Exists(FilePath)){
                using var stream = File.OpenText(FilePath);
                string line;
                ulong index = 0UL;
                while ((line = stream.ReadLine()) != null)
                {
                    TranslatedCodeLines.AddOrUpdate(
                        index++,
                        line,
                        (_0, _1) => line);
                }
            }
        }
    }
}
