using System.Threading.Tasks;
using CodeTranslator.Model;

namespace CodeTranslator.Github
{
    internal static class TaskAwaitExtension
    {
        internal static T AwaitTask<T>(this GithubAPIInfo _, Task<T> task)
        {
            task.Wait();
            return task.Result;
        }

        internal static T AwaitTask<T>(this GithubTreeItem _, Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}
