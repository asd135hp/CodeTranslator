using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeTranslator.IO
{
    public class LocalDirectoryInfo : IDirectoryInfo
    {
        private readonly DirectoryInfo _info;

        public bool Exists => _info.Exists;
        public string FullName => _info.FullName;
        public string Name => _info.Name;
        public string Extension => _info.Extension;

        public LocalDirectoryInfo(string path)
        {
            _info = new DirectoryInfo(path);
        }

        private LocalDirectoryInfo(DirectoryInfo info)
        {
            _info = info;
        }

        public async Task<IEnumerable<IDirectoryInfo>> EnumerateDirectories()
            => await Task.Run(() =>
            _info.EnumerateDirectories()
                .Select(dirInfo => new LocalDirectoryInfo(dirInfo)));

        public async Task<IEnumerable<IReadonlyFileInfo>> EnumerateFiles(
            params string[] acceptedExtensions)
            => await Task.Run(() =>
            _info.EnumerateFiles()
                .Select(fileInfo =>
                {
                    var localInfo = new LocalFileInfo(fileInfo);
                    return acceptedExtensions.Contains(localInfo.Extension) ?
                        localInfo : null;
                }));

        public async Task<IDirectoryInfo[]> GetDirectories()
            => (await EnumerateDirectories()).ToArray();

        public async Task<IReadonlyFileInfo[]> GetFiles(params string[] acceptedExtensions)
            => (await EnumerateFiles(acceptedExtensions)).ToArray();
    }
}
