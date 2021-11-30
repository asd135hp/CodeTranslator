using System.IO;
using System.Collections.Generic;

using CodeTranslator.Tree;

namespace CodeTranslator.Model
{
    public class CodeFile
    {
        internal FileInfo Info { get; private set; }
        public DirectoryTree CurrentDirectory { get; internal set; }
        public IEnumerable<string> CodeLines { get; private set; }
        public int LineCount { get; private set; }

        public CodeFile(FileInfo info)
        {
            if (info == null)
                throw new IOException("Could not get info of code file");

            var readerStream = info.OpenText();
            var codeLines = new List<string>();

            while (true)
            {
                var codeLine = readerStream.ReadLine();
                if (codeLine == null) break;
                codeLines.Add(codeLine);
            }

            Info = info;
            CodeLines = codeLines;
            LineCount = codeLines.Count;
        }
    }
}
