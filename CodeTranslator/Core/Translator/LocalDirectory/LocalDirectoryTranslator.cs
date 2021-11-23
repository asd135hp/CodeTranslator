using System;
using System.Collections.Generic;
using System.Text;

using CodeTranslator.Core.Translator.Model;

namespace CodeTranslator.Core.Translator.LocalDirectory
{
    internal class LocalDirectoryTranslator : ITranslator
    {
        public DirectoryTree Directory { get; set; }
        public Language Language { get; set; }
    }
}
