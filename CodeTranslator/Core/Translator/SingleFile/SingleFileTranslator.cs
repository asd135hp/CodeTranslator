using System;
using System.Collections.Generic;
using System.Text;

using CodeTranslator.Core.Translator.Model;
using CodeTranslator.Core.Translation.Code.Model;

namespace CodeTranslator.Core.Translator.SingleFile
{
    internal class SingleFileTranslator: ITranslator
    {
        public DirectoryTree Directory { get; set; }
        public Language Language { get; set; }
    }
}
