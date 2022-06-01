using System.Threading;
using System.Threading.Tasks;

namespace CodeTranslator.Core.Output
{
    public interface IOutput
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetContent();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] GetContentLines();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetContentAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string[]> GetContentLinesAsync(CancellationToken cancellationToken = default);
    }
}
