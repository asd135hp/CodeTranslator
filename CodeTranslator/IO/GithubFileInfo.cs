using System;
using System.IO;
using System.Linq;

using CodeTranslator.Model;

namespace CodeTranslator.IO
{
    public sealed class GitHubFileInfo : GitHubTreeItem, IReadonlyFileInfo, IDisposable
    {
        private const string STORING_FOLDER = "tmp";

        private readonly FileName _fileName;
        private readonly string _filePath;

        public GitHubDirectoryInfo Directory { get; internal set; }
        public FileInfo FileInfo => new FileInfo(_filePath);

        public override string Name => _fileName.Name;
        public override string Extension => _fileName.Extension;
        public string FullName => _fileName.FullName;

        /// <summary>
        /// Public (and private for specific user) GitHub file reader
        /// </summary>
        /// <param name="GitHubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        public GitHubFileInfo(
            string GitHubUrl,
            string commitReference,
            string accessToken)
            : this(new Uri(GitHubUrl), commitReference, accessToken, "") { }

        /// <summary>
        /// Public (and private for specific user) GitHub file reader
        /// </summary>
        /// <param name="GitHubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        public GitHubFileInfo(
            Uri GitHubUrl,
            string commitReference,
            string accessToken)
            : this(GitHubUrl, commitReference, accessToken, "") { }

        /// <summary>
        /// Public (and private for specific user) GitHub file reader
        /// </summary>
        /// <param name="GitHubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        /// <param name="branch"></param>
        public GitHubFileInfo(
            string GitHubUrl,
            string commitReference,
            string accessToken,
            string branch)
            : this(new Uri(GitHubUrl), commitReference, accessToken, branch) { }

        /// <summary>
        /// Public (and private for specific user) GitHub file reader
        /// </summary>
        /// <param name="GitHubUrl"></param>
        /// <param name="branch"></param>
        /// <param name="accessToken"></param>
        /// <param name="branch"></param>
        public GitHubFileInfo(
            Uri GitHubUrl,
            string commitReference,
            string accessToken,
            string branch)
            : this(new GitHubAPIInfo()
                  .SetAccessToken(accessToken)
                  .SetGitHubUrl(GitHubUrl)
                  .SetCommit(commitReference)
                  .SetBranch(branch)) { }

        public GitHubFileInfo(GitHubAPIInfo apiInfo) : base(apiInfo)
        {
            var segments = apiInfo.Url.Segments;
            // set up name and extension of this tree item if possible
            if (Exists)
            {
                if (segments.Length > 5)
                    _fileName = new FileName(apiInfo.Url.LocalPath);

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

        public void OpenText(Action<StreamReader> fileReader)
        {
            using var stream = File.OpenText(_filePath);
            fileReader.Invoke(stream);
        }

        public void OpenRead(Action<FileStream> fileReader)
        {
            using var stream = File.OpenRead(_filePath);
            fileReader.Invoke(stream);
        }

        public void Dispose()
        {
            File.Delete(_filePath);
        }
    }
}
