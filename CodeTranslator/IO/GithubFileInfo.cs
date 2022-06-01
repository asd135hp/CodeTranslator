using System;
using System.IO;
using System.Linq;
using Octokit;

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
            GitHubClient client,
            string githubUrl,
            string commitReference)
            : this(client, new Uri(githubUrl), commitReference, "") { }

        /// <summary>
        /// Public (and private for specific user) GitHub file reader
        /// </summary>
        /// <param name="GitHubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        public GitHubFileInfo(
            GitHubClient client,
            Uri githubUrl,
            string commitReference)
            : this(client, githubUrl, commitReference, "") { }

        /// <summary>
        /// Public (and private for specific user) GitHub file reader
        /// </summary>
        /// <param name="GitHubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        /// <param name="branch"></param>
        public GitHubFileInfo(
            GitHubClient client,
            string githubUrl,
            string commitReference,
            string branch)
            : this(client, new Uri(githubUrl), commitReference, branch) { }

        /// <summary>
        /// Public (and private for specific user) GitHub file reader
        /// </summary>
        /// <param name="GitHubUrl"></param>
        /// <param name="branch"></param>
        /// <param name="accessToken"></param>
        /// <param name="branch"></param>
        public GitHubFileInfo(
            GitHubClient client,
            Uri githubUrl,
            string commitReference,
            string branch)
            : this(new GitHubAPIInfo(client)
            {
                Url = githubUrl,
                CommitSHA = commitReference,
                Branch = branch,
            }) { }

        public GitHubFileInfo(GitHubAPIInfo apiInfo) : base(apiInfo)
        {
            var segments = apiInfo.Url.Segments;
            // set up name and extension of this tree item if possible
            if (Exists)
            {
                // start fetching content of the file
                if (segments.Length > 5)
                    _fileName = new FileName(apiInfo.Url.LocalPath);

                // create temp file and directory if not exists
                _filePath = $"{STORING_FOLDER}\\{apiInfo.TreeSHA}.tmp";
                System.IO.Directory.CreateDirectory(STORING_FOLDER);

                // clear cache for appending text
                if (File.Exists(_filePath))
                    File.WriteAllText(_filePath, "");

                // fetch content
                var fileContent = apiInfo.Client
                    .Repository
                    .Content
                    .GetAllContents(apiInfo.Owner, apiInfo.RepositoryName, AbsolutePath);

                // append all text to the temp file
                foreach (var repoContent in this.AwaitTask(fileContent))
                    File.AppendAllText(_filePath, repoContent.Content ?? "");
            }
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
