using System;
using System.IO;

namespace CodeTranslator.IO
{
    public interface IReadonlyFileInfo
    {
        void OpenText(Action<StreamReader> fileReader);
        void OpenRead(Action<FileStream> fileReader);
    }
}
