using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;
using CodeTranslator.IO;
using CodeTranslator.Utility;

namespace CodeTranslatorTest
{
    internal class TestGithubTreeItemEnumerator
    {
        private const int DEPTH = 255;
        private string githubRepo = "https://github.com/graphql-dotnet/graphql-dotnet";
        private string commitSHA = "a4803cf69cf083a6754e52cdb7b0ade5e5094375";
        private string accessToken = File.ReadAllText(
            $"{GetDirectory.ProjectDirectory}\\token.txt");

        private async Task EnumerateInfo(GithubDirectoryInfo rootDir, bool recursive = false, int depth = 0)
        {
            if (depth > DEPTH) return;

            var dirs = await rootDir.EnumerateDirectories();
            foreach (GithubDirectoryInfo dirInfo in dirs)
            {
                TestContext.Out.WriteLine(
                    "Absolute path: {0}; Name: {1}",
                    dirInfo.AbsolutePath,
                    dirInfo.Name);
                if (recursive) await EnumerateInfo(dirInfo, true, depth + 1);
            }

            var files = await rootDir.EnumerateFiles();
            foreach (GithubFileInfo dirInfo in files)
            {
                TestContext.Out.WriteLine(
                    "Absolute path: {0}; Name: {1}",
                    dirInfo.AbsolutePath,
                    dirInfo.Name + '.' + dirInfo.Extension);

                Assert.AreEqual(
                    dirInfo.AbsolutePath,
                    $"{dirInfo.Directory.AbsolutePath}/{dirInfo.Name}.{dirInfo.Extension}"
                );
            }
        }

        [Test]
        public void TestOneLevelEnumeration()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                await EnumerateInfo(
                    new GithubDirectoryInfo(githubRepo, commitSHA, accessToken)
                );
            });
        }

        [Test]
        public void TestShortRecursiveEnumeration()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                await EnumerateInfo(
                    new GithubDirectoryInfo(githubRepo, commitSHA, accessToken),
                    true,
                    252
                );
            });
        }
    }
}
