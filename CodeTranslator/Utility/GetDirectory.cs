using System.IO;

namespace CodeTranslator.Utility
{
    public static class GetDirectory
    {
        private static DirectoryInfo GetInfo(string directory)
            => Directory.GetParent(directory);

        private static string GetDirectoryFullName(int parentCount)
        {
            var info = GetInfo(CurrentDirectory);
            while(true)
            {
                info = info.Parent;
                if (--parentCount == 0) return info.FullName;
            }
        }

        public static string CurrentDirectory => Directory.GetCurrentDirectory();

        /// <summary>
        /// Debugging purposes. Don't use this in Release.
        /// </summary>
        public static string BinaryDirectory => GetDirectoryFullName(1);

        /// <summary>
        /// Debugging purposes. Don't use this in Release.
        /// </summary>
        public static string ProjectDirectory => GetDirectoryFullName(2);
    }
}
