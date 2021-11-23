using NUnit.Framework;
using CodeTranslator.Utility;
using CodeTranslator.Core.Translation.Code.Model;

namespace CodeTranslatorTest
{
    public class Tests
    {
        private Language language;

        [SetUp]
        public void Setup()
        {
            language = Language.Builder()
                .SetDirectory(GetDirectory.ProjectDirectory)
                .SetLanguageFile("vi.json")
                .Build();
        }

        [Test]
        public void CheckTranslatedKeyWords()
        {
            Assert.AreEqual("Vietnamese", language.LanguageName);
            Assert.AreEqual("so nguyen", language.TranslatedKeywords["int"]);
        }
    }
}