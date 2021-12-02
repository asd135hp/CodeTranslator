using NUnit.Framework;
using System;
using System.IO;
using System.Threading;

using CodeTranslator;
using CodeTranslator.IO;
using CodeTranslator.Core.Translation;
using CodeTranslator.Core.Translation.Code;
using CodeTranslator.Model;
using CodeTranslator.Utility;
using CodeTranslator.Utility.Progress;

namespace CodeTranslatorTest
{
    public class TestTranslation
    {
        private const string TestFolder = "TestResources";
        private static readonly string
            TestPath = $@"{GetDirectory.ProjectDirectory}\{TestFolder}",
            LanguageFile = $@"{TestPath}\vi-csharp.json",
            TranslationFile = $@"{TestPath}\viTestFile.cstranslation",
            TestFile = $@"{TestPath}\TestFile.cs";
        private Language language;
        private ITranslation translation;
        private CodeFile file;

        public void Setup(bool reverseTranslation = false, string codeFileDir = null)
        {
            language = Language.Builder
                .SetLanguageFileName(LanguageFile)
                .SetReverseTranslation(reverseTranslation)
                .Build();

            translation = new CodeTranslation()
            {
                Language = language
            };

            file = new CodeFile(new LocalFileInfo(codeFileDir ?? TestFile));
        }

        [Test]
        public void CheckTranslatedKeyWords()
        {
            // direct translation
            Setup();
            Assert.AreEqual("Vietnamese", language.LanguageName);
            Assert.AreEqual("số nguyên", language.TranslatedKeywords["int"]);
            Assert.AreEqual("số nguyên ngắn", language.TranslatedKeywords["short"]);
            Assert.AreEqual("nguyên dương ngắn", language.TranslatedKeywords["ushort"]);

            // reverse translation
            Setup(true);
            Assert.AreEqual("Vietnamese", language.LanguageName);
            Assert.AreEqual("ulong", language.TranslatedKeywords["nguyên dương dài"]);
            Assert.AreEqual("uint", language.TranslatedKeywords["nguyên dương"]);
            Assert.AreEqual("string", language.TranslatedKeywords["xâu"]);
        }

        [Test]
        public void TestCodeTranslation()
        {
            Setup();
            var outputFileName = translation.GetOutput(file).SaveOutput();
            var progress = translation.GetObservableProgressTracker() as ProgressTracker;
            while(!progress?.IsFinished ?? false)
            {
                Thread.Sleep(1000);
            }

            using var stream = File.OpenRead(outputFileName);
            using var otherStream = File.OpenRead(TranslationFile);
            Console.WriteLine("Similarity rating: {0}%", stream.SimilarityRating(otherStream) * 100);
            Assert.AreEqual(true, stream.Compare(otherStream));
        }

        [Test]
        public void TestReverseCodeTranslation()
        {
            Setup(true, TranslationFile);
            var outputFileName = translation.GetOutput(file).SaveOutput();
            var progress = translation.GetObservableProgressTracker() as ProgressTracker;
            while (!progress?.IsFinished ?? false)
            {
                Thread.Sleep(1000);
            }

            using var stream = File.OpenRead(outputFileName);
            using var otherStream = File.OpenRead(TestFile);
            Console.WriteLine("Similarity rating: {0}%", stream.SimilarityRating(otherStream) * 100);
            Assert.AreEqual(true, stream.Compare(otherStream));
        }
    }
}