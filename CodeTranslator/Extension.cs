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

        public static T Await<T>(this Task<T> task)
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
        public static bool Compare(this StreamReader reader, StreamReader otherReader)
            => Math.Floor(reader.SimilarityRating(otherReader)) == 1.0;

        public static double SimilarityRating(this StreamReader reader, StreamReader otherReader)
        {
            long dist = reader.NaiveDiffComparision(otherReader);

            reader.BaseStream.Position = otherReader.BaseStream.Position = 0;

            return 1 - (double)dist / Math.Max(reader.BaseStream.Length, otherReader.BaseStream.Length);
        }

        private static long LevenshteinDistance(this StreamReader reader, StreamReader otherReader)
        {

            return 0;
        }

        private static long NaiveDiffComparision(this StreamReader reader, StreamReader otherReader)
        {
            long diffCount = 0L;
            int currentChar, otherChar;

            while (true)
            {
                currentChar = reader.Read();
                otherChar = otherReader.Read();

                if (currentChar == -1 || otherChar == -1) break;

                if (currentChar != otherChar) diffCount += 1;
            }

            return diffCount;
        }
        #endregion

        #endregion
    }
}
