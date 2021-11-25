using System;
using System.Collections.Generic;
using System.IO;

namespace CodeTranslator.Model
{
    public sealed class GithubDirectoryInfo : FileSystemInfo
    {
        public GithubDirectoryInfo(Uri uri) : base()
        {
            Attributes |= FileAttributes.Directory;


        } 

        public override bool Exists => throw new NotImplementedException();

        public override string Name => throw new NotImplementedException();

        public override void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
