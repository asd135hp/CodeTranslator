using System.IO;

namespace CodeTranslator.Core.Output
{
    public interface IOutput
    {
        string GetCurrent();
        bool MoveNext();
    }
}
