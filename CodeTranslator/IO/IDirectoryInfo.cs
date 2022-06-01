using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeTranslator.IO
{
    public interface IDirectoryInfo
    {
        /// <summary>
        /// Represents name of this directory
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Represents the absolute path that this directory lies in
        /// </summary>
        string FullName { get; }

        Task<IEnumerable<IDirectoryInfo>> EnumerateDirectories();
        Task<IDirectoryInfo[]> GetDirectories();
        Task<IEnumerable<IReadonlyFileInfo>> EnumerateFiles(params string[] acceptedExtensions);
        Task<IReadonlyFileInfo[]> GetFiles(params string[] acceptedExtensions);
    }
}
