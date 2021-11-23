﻿using System;
using System.Collections.Generic;
using System.Text;

using CodeTranslator.Core.Translator.Model;
using CodeTranslator.Core.Translation.Code.Model;

namespace CodeTranslator.Core.Translator.Github
{
    internal class GithubTranslator: ITranslator
    {
        public DirectoryTree Directory { get; set; }
        public Language Language { get; set; }
    }
}
