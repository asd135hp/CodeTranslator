using System;
using System.Collections.Generic;
using System.IO;

namespace CodeTranslator.Model.Github
{
    public sealed class GithubFileInfo : FileSystemInfo
    {
        public override bool Exists => throw new NotImplementedException();

        public override string Name => throw new NotImplementedException();

        public override void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
