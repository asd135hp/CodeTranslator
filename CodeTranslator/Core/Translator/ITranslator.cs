using CodeTranslator.IO;
using CodeTranslator.Core.Tree;
using CodeTranslator.Core.Output;
using System.Collections.Generic;

namespace CodeTranslator.Core.Translator
{
    public interface ITranslator<TDirectoryInfo, TFileInfo>
        where TDirectoryInfo : IDirectoryInfo
        where TFileInfo : IReadonlyFileInfo
    {
        /// <summary>
        /// Enumerate provided directory tree (all, not on demand)
        /// </summary>
        /// <returns></returns>
        public DirectoryTree<TDirectoryInfo, TFileInfo> GetDirectoryTree();

        /// <summary>
        /// Translate a single code file from the given file info
        /// </summary>
        /// <returns></returns>
        public IOutput TranslateFile(IReadonlyFileInfo file);

        /// <summary>
        /// Translate all possible code files unconditionally. Not recommended for a super large project
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IOutput> TranslateFiles();
    }
}
