using System;
using System.IO;
using System.Threading.Tasks;
using CodeTranslator.IO;
using CodeTranslator.Model;

namespace CodeTranslator
{
    public static class Extension
    {
        #region Threading

        #region Tasks
        internal static T AwaitTask<T>(this GitHubAPIInfo _, Task<T> task)
        {
            task.Wait();
            return task.Result;
        }

        internal static T AwaitTask<T>(this GitHubTreeItem _, Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
        #endregion

        #endregion

        #region Primitive

        #region String
        public static byte[] ToByteArray(this string str)
        {
            var byteArr = new byte[str.Length];
            var index = 0;
            foreach (char c in str) byteArr[index++] = (byte)c;
            return byteArr;
        }

        /// <summary>
        /// Trim the specified trailing substring out of the original string
        /// </summary>
        /// <see cref="https://stackoverflow.com/questions/4335878/"/>
        /// <param name="str"></param>
        /// <param name="trimString"></param>
        /// <returns></returns>
        public static string TrimEnd(this string str, string trimString)
        {
            if (string.IsNullOrEmpty(trimString)) return str;

            string result = str;
            while (result.EndsWith(trimString))
            {
                result = result[..^trimString.Length];
            }
            return result;
        }

        /// <summary>
        /// Trim the specified leading substring out of the original string
        /// </summary>
        /// <see cref="https://stackoverflow.com/questions/4335878/"/>
        /// <param name="str"></param>
        /// <param name="trimString"></param>
        /// <returns></returns>
        public static string TrimStart(this string str, string trimString)
        {
            if (string.IsNullOrEmpty(trimString)) return str;

            string result = str;
            while (result.StartsWith(trimString))
            {
                result = result[trimString.Length..];
            }
            return result;
        }

        /// <summary>
        /// Trim the specified substring out of the original string on both sides
        /// </summary>
        /// <see cref="https://stackoverflow.com/questions/4335878/"/>
        /// <param name="str"></param>
        /// <param name="trimString"></param>
        /// <returns></returns>
        public static string Trim(this string str, string trimString)
        {
            if (string.IsNullOrEmpty(trimString)) return str;
            return str.TrimStart(trimString).TrimEnd(trimString);
        }

        public static int LevenshteinDistance(this string str, string comparingString)
        {
            // well, dunno if this is helpful for the project or not...
            return 0;
        }
        #endregion

        #endregion

        #region IO

        #region File
        public static bool Compare(this Stream stream, Stream otherStream)
            => stream.SimilarityRating(otherStream) == 1.0;

        public static double SimilarityRating(this Stream stream, Stream otherStream)
        {
            long similarityCount = 0L;
            int currentByte, otherByte;

            while(true)
            {
                currentByte = stream.ReadByte();
                otherByte = otherStream.ReadByte();
                if (currentByte == -1 || otherByte == -1) break;

                Console.Write("{0},", currentByte);
                if (currentByte == otherByte) similarityCount += 1;
            }

            stream.Position = otherStream.Position = 0;

            return (double)similarityCount / Math.Max(stream.Length, otherStream.Length);
        }
        #endregion

        #endregion
    }
}
