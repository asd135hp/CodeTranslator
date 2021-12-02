using System.Linq;

namespace CodeTranslator.Model
{
    internal class FileName
    {
        public readonly string FullName, Name, Extension;
        
        public FileName(string path)
        {
            var nameParts = path.Split('/', '\\').Last().Split('.').ToList();
            var namePrefix = path[0] == '.' && nameParts[0].Length != 0 ? "." : "";

            Extension = nameParts.Last();
            nameParts.RemoveAt(nameParts.Count - 1);
            Name = $"{namePrefix}{string.Join(".", nameParts)}";
            FullName = path;
        }
    }
}
