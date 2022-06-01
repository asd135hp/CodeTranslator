using CodeTranslator.Core.Output;

namespace CodeTranslator.Core.Parser
{
    public interface IParser
    {
        /// <summary>
        /// Start parsing the provided content into a tree of tokens (Abstract Syntax Tree or any tree)
        /// </summary>
        /// <param name="content"></param>
        void Parse(string content);

        /// <summary>
        /// Translate words parsed by this object (combine all keywords into a single file)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        IOutput Translate(string fileName);
    }
}
