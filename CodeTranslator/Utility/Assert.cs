using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTranslator.Utility
{
    internal static class Assert
    {
        public static void ValueNotNull<T>(T argVal)
        {
            if (argVal == null)
                throw new NullReferenceException(
                    $"Expected value of type: {nameof(argVal)} but null is passed instead");
        }

        public static void ArgumentNotNull<T>(T argVal)
        {
            if(argVal == null)
                throw new ArgumentNullException(
                    $"Expected value of type: {nameof(argVal)} but null is passed instead");
        }

        public static void StringNotNullOrEmpty(string strVal)
        {
            ValueNotNull(strVal);
        }
    }
}
