using System;
using System.IO;
using System.Threading.Tasks;

using CodeTranslator.Common;
using CodeTranslator.Core.Translation;
using CodeTranslator.IO;
using CodeTranslator.Model;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace CodeTranslator.Core.Translator
{
    public class ZipFileTranslator : AbstractTranslator<LocalDirectoryInfo, LocalFileInfo>
    {
        private readonly string _generatedDir;
        private ZipFileProgressStatus _progress;

        public ZipFileProgressStatus Progress => _progress;

        public override TranslatorType Type => TranslatorType.ZipFile;

        public ZipFileTranslator(string zipFilePath, ITranslation translation)
            : base(translation)
        {
            using var stream = File.OpenRead(zipFilePath);
            _generatedDir = $@".\{new Guid(zipFilePath.ToByteArray())}";
            _progress = new ZipFileProgressStatus(stream.Length);
            ReadZipfile(stream);
        }

        private void ReadZipfile(Stream stream)
            => Task.Run(() =>
            {
                using var reader = ReaderFactory.Open(stream);
                reader.EntryExtractionProgress += ReadProgress;

                Directory.CreateDirectory(_generatedDir);
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                        reader.WriteEntryToDirectory(_generatedDir, new ExtractionOptions()
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
                }
                _rootDirectory = new LocalDirectoryTree(_generatedDir);
            });

        private void ReadProgress(object _, ReaderExtractionEventArgs<IEntry> entry)
        {
            var progress = entry.ReaderProgress;
            _progress.CurrentExtractedBytes = progress.BytesTransferred;

            if (progress.PercentageReadExact == 100.0)
                _progress.TotalExtractedBytes += progress.BytesTransferred;
        }
    }
}
