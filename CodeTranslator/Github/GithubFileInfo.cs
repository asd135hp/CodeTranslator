using System;
using System.IO;
using System.Linq;

using CodeTranslator.Model;

namespace CodeTranslator.Github
{
    public sealed class GithubFileInfo : GithubTreeItem, IDisposable
    {
        private const string STORING_FOLDER = "tmp";

        private readonly FileName _fileName;
        private readonly string _filePath;

        public GithubDirectoryInfo Directory { get; internal set; }

        public override string Name => _fileName.Name;
        public override string Extension => _fileName.Extension;

        /// <summary>
        /// Public (and private for specific user) Github file reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        public GithubFileInfo(
            string githubUrl,
            string commitReference,
            string accessToken)
            : this(new Uri(githubUrl), commitReference, accessToken, "") { }

        /// <summary>
        /// Public (and private for specific user) Github file reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        public GithubFileInfo(
            Uri githubUrl,
            string commitReference,
            string accessToken)
            : this(githubUrl, commitReference, accessToken, "") { }

        /// <summary>
        /// Public (and private for specific user) Github file reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        /// <param name="branch"></param>
        public GithubFileInfo(
            string githubUrl,
            string commitReference,
            string accessToken,
            string branch)
            : this(new Uri(githubUrl), commitReference, accessToken, branch) { }

        /// <summary>
        /// Public (and private for specific user) Github file reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="branch"></param>
        /// <param name="accessToken"></param>
        /// <param name="branch"></param>
        public GithubFileInfo(
            Uri githubUrl,
            string commitReference,
            string accessToken,
            string branch)
            : this(new GithubAPIInfo()
                  .SetAccessToken(accessToken)
                  .SetGithubUrl(githubUrl)
                  .SetCommit(commitReference)
                  .SetBranch(branch)) { }

        public GithubFileInfo(GithubAPIInfo apiInfo) : base(apiInfo)
        {
            var segments = apiInfo.Url.Segments;
            // set up name and extension of this tree item if possible
            if (Exists)
            {
                if (segments.Length > 5)
                    _fileName = new FileName(segments.Last());

                // start fetching content of the file
                _filePath = $"{STORING_FOLDER}\\{apiInfo.TreeSHA}.tmp";
                System.IO.Directory.CreateDirectory(STORING_FOLDER);
                if (File.Exists(_filePath))
                    File.WriteAllText(_filePath, "");

                foreach (var repoContent in this.AwaitTask(apiInfo.FetchFileContent(AbsolutePath)))
                    File.AppendAllText(_filePath, repoContent.Content ?? "");
            }
            else _exists = false;
        }

        public void ReadFileContents(Action<StreamReader> fileReader)
        {
            using var stream = File.OpenText(_filePath);
            fileReader.Invoke(stream);
        }

        public void Dispose()
        {
            File.Delete(_filePath);
        }
    }
}
