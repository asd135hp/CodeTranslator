using CodeTranslator.Common;
using CodeTranslator.Core.Parser;
using CodeTranslator.Core.Tree;
using CodeTranslator.Core.Output;
using CodeTranslator.IO;
using System.Linq;
using System.Collections.Generic;

namespace CodeTranslator.Core.Translator
{
    public abstract class AbstractTranslator<TDirectoryInfo, TFileInfo>
        : ITranslator<TDirectoryInfo, TFileInfo>
        where TDirectoryInfo : IDirectoryInfo
        where TFileInfo : IReadonlyFileInfo
    {
        private readonly IParser _parser;
        protected DirectoryTree<TDirectoryInfo, TFileInfo> _rootDirectory;

        public abstract TranslatorType Type { get; }

        public AbstractTranslator(IParser parser)
        {
            _parser = parser;
        }

        public AbstractTranslator(DirectoryTree<TDirectoryInfo, TFileInfo> rootDirectory, IParser parser)
            : this(parser)
        {
            _rootDirectory = rootDirectory;
        }

        public DirectoryTree<TDirectoryInfo, TFileInfo> GetDirectoryTree()
            => _rootDirectory;

        public IEnumerable<IOutput> TranslateFiles()
            => _rootDirectory.Files.Select(file => TranslateFile(file));

        public IOutput TranslateFile(IReadonlyFileInfo file)
        {
            file.OpenText(reader => _parser.Parse(reader.ReadToEnd()));
            return _parser.Translate(file.Name);
        }
    }
}
