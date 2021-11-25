using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CodeTranslator.Model;
using CodeTranslator.Model.Tree;

namespace CodeTranslator.Core.Translator.Github
{
    public sealed class GithubDirectoryTree : DirectoryTree
    {
        private const int MAX_DEPTH = 255;
        
        public GithubDirectoryTree(string rootDirectoryPath) : base(rootDirectoryPath)
        {
        }

        private GithubDirectoryTree(string rootDirectoryPath, DirectoryTree parentDirectory)
            : base(rootDirectoryPath, parentDirectory)
        {
        }

        internal override IEnumerable<DirectoryInfo> EnumerateDirectories(string path)
        {
            throw new NotImplementedException();
        }

        internal override void PopulateDirectories()
        {
            if (Depth >= MAX_DEPTH) return;
            throw new NotImplementedException();
        }

        internal override IEnumerable<FileInfo> EnumerateFiles(string path)
        {
            throw new NotImplementedException();
        }
    }
}
