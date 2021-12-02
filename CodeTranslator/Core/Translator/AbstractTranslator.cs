using CodeTranslator.Common;
using CodeTranslator.Core.Output;
using CodeTranslator.Core.Translation;
using CodeTranslator.Core.Tree;
using CodeTranslator.IO;
using CodeTranslator.Model;

namespace CodeTranslator.Core.Translator
{
    public abstract class AbstractTranslator<TDirectoryInfo, TFileInfo>
        : ITranslator<TDirectoryInfo, TFileInfo>
        where TDirectoryInfo : IDirectoryInfo
        where TFileInfo : IReadonlyFileInfo
    {
        private readonly ITranslation _translation;
        protected DirectoryTree<TDirectoryInfo, TFileInfo> _rootDirectory;

        public abstract TranslatorType Type { get; }

        public AbstractTranslator(ITranslation translation)
        {
            _translation = translation;
        }

        public AbstractTranslator(
            DirectoryTree<TDirectoryInfo, TFileInfo> rootDirectory,
            ITranslation translation)
            : this(translation)
        {
            _rootDirectory = rootDirectory;
        }

        public DirectoryTree<TDirectoryInfo, TFileInfo> GetDirectoryTree()
            => _rootDirectory;

        public IOutput TranslateFile(CodeFile codeFile)
            => _translation.GetOutput(codeFile);
    }
}
