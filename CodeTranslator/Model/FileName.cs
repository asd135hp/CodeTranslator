using System.Linq;

namespace CodeTranslator.Model
{
    internal class FileName
    {
        public readonly string Name, Extension;
        
        public FileName(string fileName)
        {
            var nameParts = fileName.Split('.').ToList();
            var namePrefix = fileName[0] == '.' && nameParts[0].Length != 0 ? "." : "";

            Extension = nameParts.Last();
            nameParts.RemoveAt(nameParts.Count - 1);
            Name = $"{namePrefix}{string.Join(".", nameParts)}";
        }
    }
}
