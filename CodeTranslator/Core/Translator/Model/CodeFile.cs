using System.Collections.Generic;

namespace CodeTranslator.Core.Translator.Model
{
    internal class CodeFile: CustomFileInfo
    {
        public DirectoryTree ParentFolder { get; internal set; }
        public IEnumerable<string> CodeLines { get; internal set; }
        public uint LineCount { get; internal set; }
    }
}
