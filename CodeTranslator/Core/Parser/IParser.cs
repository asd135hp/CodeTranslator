using System.Collections.Generic;
using CodeTranslator.Model;
using CodeTranslator.Rules;

namespace CodeTranslator.Core.Parser
{
    internal interface IParser
    {
        IEnumerable<IRule> Rules { get; set; }

        CodeTranslationSettings Settings { get; set; }

        /// <summary>
        /// Start parsing the provided content
        /// </summary>
        /// <param name="content"></param>
        void Parse(string content);

        /// <summary>
        /// Translate words parsed by this object
        /// </summary>
        /// <returns></returns>
        string Translate();
    }
}
