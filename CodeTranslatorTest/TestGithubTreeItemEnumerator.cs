using NUnit.Framework;
using System;
using System.Threading.Tasks;
using CodeTranslator.Model.Github;

namespace CodeTranslatorTest
{
    internal class TestGithubTreeItemEnumerator
    {
        private string githubRepo = "https://github.com/graphql-dotnet/graphql-dotnet";
        private string commitSHA = "a4803cf69cf083a6754e52cdb7b0ade5e5094375";
        private string accessToken = "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";

        private async Task EnumerateInfo(GithubDirectoryInfo rootDir, bool recursive = false)
        {
            foreach (var dirInfo in await rootDir.EnumerateDirectories())
            {
                TestContext.Out.WriteLine(
                    "Absolute path: {0}; Name: {1}",
                    dirInfo.AbsolutePath,
                    dirInfo.Name + '.' + dirInfo.Extension);
                if (recursive) await EnumerateInfo(dirInfo, true);
            }

            foreach (var dirInfo in await rootDir.EnumerateFiles())
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
                await EnumerateInfo(new GithubDirectoryInfo(githubRepo, accessToken)
                {
                    CommitSHA = commitSHA
                });
            });
        }

        [Test]
        public void TestRecursiveEnumeration()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                await EnumerateInfo(new GithubDirectoryInfo(githubRepo, accessToken)
                {
                    CommitSHA = commitSHA
                }, true);
            });
        }
    }
}
