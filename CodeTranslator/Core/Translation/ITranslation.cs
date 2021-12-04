using System;
using CodeTranslator.Core.Output;
using CodeTranslator.Model;

namespace CodeTranslator.Core.Translation
{
    public interface ITranslation
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<TranslationProgressEventArgs> TranslationTracker;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeFile"></param>
        /// <returns></returns>
        IOutput GetOutput(CodeFile codeFile);
    }
}
