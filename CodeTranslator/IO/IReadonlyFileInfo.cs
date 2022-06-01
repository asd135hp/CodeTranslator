using System;
using System.IO;

namespace CodeTranslator.IO
{
    public interface IReadonlyFileInfo
    {
        /// <summary>
        /// Represents the name of this file without extension
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Represents the extension of this file
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Represents the absolute path that the file lies in 
        /// </summary>
        string FullName { get; }

        void OpenText(Action<StreamReader> fileReader);
        void OpenRead(Action<FileStream> fileReader);
    }
}
