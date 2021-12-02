using System;
using System.IO;

namespace CodeTranslator.IO
{
    public interface IReadonlyFileInfo
    {
        string Name { get; }
        string Extension { get; }
        string FullName { get; }

        void OpenText(Action<StreamReader> fileReader);
        void OpenRead(Action<FileStream> fileReader);
    }
}
