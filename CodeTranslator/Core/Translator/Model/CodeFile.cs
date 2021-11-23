using System.IO;
using System.Collections.Generic;

namespace CodeTranslator.Core.Translator.Model
{
    public abstract class CodeFile
    {
        internal FileInfo Info { get; set; }
        public DirectoryTree ParentDirectory { get; internal set; }
        public IEnumerable<string> CodeLines { get; internal set; }
        public uint LineCount { get; internal set; }
    }
}
