using NUnit.Framework;
using System;
using System.IO;

using CodeTranslator.Core.Parser;
using CodeTranslator.Core.Parser.Decomposer;
using CodeTranslator.Core.Parser.Recomposer;
using CodeTranslator.Core.Input;
using CodeTranslator.Core.Translator;
using CodeTranslator.Utility;

namespace CodeTranslatorTest
{
    public class TestTranslation
    {
        private const string TestFolder = "TestResources";
        private static readonly string
            TestPath = $@"{GetDirectory.ProjectDirectory}\{TestFolder}",
            LanguageFile = $@"{TestPath}\vi-csharp.json",
            TranslationFile = $@"{TestPath}\viTestFile.cstranslation",
            TestFile = $@"{TestPath}\TestFile.cs",
            GitHubLink = "",
            LocalDir = $@"",
            ZipFile = $@"";

        private IInput Input;

        public void Setup()
        {
            Input = new ApplicationInput()
            {
                Parser = new CodeParser(new Extractor(), new Combiner())
            };
        }

        [Test]
        public void TestGitHub()
        {
            Input.RootPath = GitHubLink;
            var translator = Input.GetGitHubTranslator("", "", "");
        }
    }
}