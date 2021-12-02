﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeTranslator.IO
{
    public interface IDirectoryInfo
    {
        string Name { get; }
        string FullName { get; }

        Task<IEnumerable<IDirectoryInfo>> EnumerateDirectories();
        Task<IDirectoryInfo[]> GetDirectories();
        Task<IEnumerable<IReadonlyFileInfo>> EnumerateFiles(params string[] acceptedExtensions);
        Task<IReadonlyFileInfo[]> GetFiles(params string[] acceptedExtensions);
    }
}
