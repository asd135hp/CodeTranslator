namespace CodeTranslator.Core.Output
{
    public interface IOutput
    {
        /// <summary>
        /// Get the currently stored string in a list of translated code lines
        /// </summary>
        /// <returns>A translated code line in comparison to its original content</returns>
        string GetCurrent();
        
        /// <summary>
        /// Move the cursor one line forward to read translated code line stored in this object
        /// </summary>
        /// <returns>False if the cursor could not move forward anymore (EOF)</returns>
        bool MoveNext();

        /// <summary>
        /// Initiate a thread-blocking work to store main content within this object to a file.
        /// </summary>
        /// <returns>
        /// A local path pointing to the file that all the translated code lines
        /// are stored to. If reverse translation is not set to true, the extension of this
        /// output file will be the extension of translation file without translation part.
        /// Otherwise, its extension will become original extension plus translation instead.
        /// </returns>
        /// <example>
        /// No reverse translation: cs -> cstranslation
        /// Reverse translation enabled: cstranslation -> cs
        /// </example>
        string SaveOutput();
    }
}
