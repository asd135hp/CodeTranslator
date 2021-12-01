using System.Threading.Tasks;
using CodeTranslator.Model;

namespace CodeTranslator.IO
{
    public static class Extension
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

        public static byte[] ToByteArray(this string str)
        {
            var byteArr = new byte[str.Length];
            var index = 0;
            foreach(char c in str) byteArr[index++] = (byte)c;
            return byteArr;
        }
    }
}
