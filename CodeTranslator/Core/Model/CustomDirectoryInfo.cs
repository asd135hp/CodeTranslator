using System;
using System.Collections.Generic;
using System.IO;

namespace CodeTranslator.Core.Model
{
    public class CustomDirectoryInfo
    {
        public IEnumerable<DirectoryInfo> Directories;
        public IEnumerable<FileInfo> Files;

        public CustomDirectoryInfo()
        {

        }

        public CustomDirectoryInfo(DirectoryInfo directoryInfo)
        {
            Directories = directoryInfo.EnumerateDirectories();
            Files = directoryInfo.EnumerateFiles();
        }
    }
}
