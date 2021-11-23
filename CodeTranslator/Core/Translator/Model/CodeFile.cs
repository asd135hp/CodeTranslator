using System.IO;
using System.Collections.Generic;

namespace CodeTranslator.Core.Translator.Model
{
    internal class CodeFile
    {
        internal FileInfo Info { get; set; }
        public DirectoryTree ParentFolder { get; internal set; }
        public IEnumerable<string> CodeLines { get; internal set; }
        public uint LineCount { get; internal set; }
    }
}
