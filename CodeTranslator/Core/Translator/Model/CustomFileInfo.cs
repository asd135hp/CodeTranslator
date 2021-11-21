using System;

namespace CodeTranslator.Core.Translator.Model
{
    internal abstract class CustomFileInfo
    {
        public string Name { get; internal set; }
        public string Extension { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetFileName(string fileName)
        {
            var splittedNames = fileName.Split('.');
            if (splittedNames.Length > 2) throw new ArgumentException("Invalid file name");

            Name = splittedNames[0];
            Extension = splittedNames[1];
        }
    }
}
