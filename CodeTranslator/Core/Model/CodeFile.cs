using System.IO;
using System.Collections.Generic;

namespace CodeTranslator.Core.Model
{
    public class CodeFile
    {
        internal FileInfo Info { get; set; }
        public DirectoryTree CurrentDirectory { get; internal set; }
        public IEnumerable<string> CodeLines { get; private set; }
        public int LineCount { get; private set; }

        public CodeFile()
        {
            if (Info == null)
                throw new IOException("Could not get info of code file");

            var readerStream = Info.OpenText();
            var codeLines = new List<string>();

            while (true)
            {
                var codeLine = readerStream.ReadLine();
                if (codeLine == null) break;
                codeLines.Add(codeLine);
            }

            CodeLines = codeLines;
            LineCount = codeLines.Count;
        }
    }
}
