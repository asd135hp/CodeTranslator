using System;
using System.Collections.Generic;

using CodeTranslator.IO;

namespace CodeTranslator.Model
{
    public class CodeFile
    {
        public IReadonlyFileInfo Info { get; private set; }
        public IEnumerable<string> CodeLines { get; private set; }
        public int LineCount { get; private set; }

        public CodeFile(IReadonlyFileInfo info)
        {
            if (info == null)
                throw new ArgumentException("A file info must be passed to this constructor");

            var codeLines = new List<string>();
            
            info.OpenText(readerStream =>
            {
                while (true)
                {
                    var codeLine = readerStream.ReadLine();
                    if (codeLine == null) break;
                    codeLines.Add(codeLine);
                }
            });

            Info = info;
            CodeLines = codeLines;
            LineCount = codeLines.Count;
        }
    }
}
