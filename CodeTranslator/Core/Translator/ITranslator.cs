using CodeTranslator.Core.Output;
using CodeTranslator.IO;
using CodeTranslator.Model;
using CodeTranslator.Core.Tree;

namespace CodeTranslator.Core.Translator
{
    public interface ITranslator<TDirectoryInfo, TFileInfo>
        where TDirectoryInfo : IDirectoryInfo
        where TFileInfo : IReadonlyFileInfo
    {
        public DirectoryTree<TDirectoryInfo, TFileInfo> GetDirectoryTree();
        public IOutput TranslateFile(CodeFile codeFile);
    }
}
