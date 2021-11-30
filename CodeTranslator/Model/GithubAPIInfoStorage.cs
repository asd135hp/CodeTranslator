using System;

namespace CodeTranslator.Model
{
    internal class GithubAPIInfoStorage
    {
        public Uri Url { get; set; }
        public string Branch { get; set; }
        public string Owner { get; set; }
        public string RepositoryName { get; set; }
        public string TreeSHA { get; set; }
        public string CommitSHA { get; set; }
        public int Timeout { get; set; }
    }
}
