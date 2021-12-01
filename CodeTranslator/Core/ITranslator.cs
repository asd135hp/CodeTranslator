using CodeTranslator.Core.Output;
using CodeTranslator.IO;
using CodeTranslator.Model;
using CodeTranslator.Tree;

namespace CodeTranslator.Core
{
    public interface ITranslator<TDirectoryInfo, TFileInfo>
        where TDirectoryInfo : IDirectoryInfo
        where TFileInfo : IReadonlyFileInfo
    {
        public DirectoryTree<TDirectoryInfo, TFileInfo> GetDirectoryTree();
        public IOutput TranslateFile(CodeFile codeFile);
    }
}
