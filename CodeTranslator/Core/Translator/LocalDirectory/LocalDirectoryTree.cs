using System;
using System.Collections.Generic;
using System.Text;
using CodeTranslator.Core.Translator.Model;

namespace CodeTranslator.Core.Translator.LocalDirectory
{
    public class LocalDirectoryTree : DirectoryTree
    {
        private const int MAX_DEPTH = 255;
        private bool _isMaxDepthReached = false;

        public LocalDirectoryTree(string rootDirectory) : base(rootDirectory)
        {
        }

        private LocalDirectoryTree(string rootDirectory, DirectoryTree parentDirectory, int depth = 0)
            : base(rootDirectory, parentDirectory)
        {
            if (depth >= MAX_DEPTH) _isMaxDepthReached = true;
        }

        protected override void GenerateChildDirectories()
        {
            var childDirectories = new List<DirectoryTree>();

            ChildDirectories = childDirectories;
        }

        private void RecursiveDirectoryIterator(DirectoryTree parent)
        {
            if (_isMaxDepthReached) return;
        }
    }
}
