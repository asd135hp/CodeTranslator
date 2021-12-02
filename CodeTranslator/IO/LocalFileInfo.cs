using System;
using System.IO;

namespace CodeTranslator.IO
{
    public class LocalFileInfo : IReadonlyFileInfo
    {
        private readonly FileInfo _info;

        public bool Exists => _info.Exists;
        public string Name => _info.Name;
        public string Extension => _info.Extension;
        public string FullName => _info.FullName;

        public LocalFileInfo(string path)
        {
            _info = new FileInfo(path);
        }

        internal LocalFileInfo(FileInfo info)
        {
            _info = info;
        }

        public void OpenRead(Action<FileStream> fileReader)
        {
            using var stream = _info.OpenRead();
            fileReader.Invoke(stream);
        }

        public void OpenText(Action<StreamReader> fileReader)
        {
            using var stream = _info.OpenText();
            fileReader.Invoke(stream);
        }
    }
}
