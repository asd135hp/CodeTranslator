using System;
using System.Collections.Generic;
using CodeTranslator.Common;

namespace CodeTranslator.Core.Parser.Components
{
    internal class LambdaFunction : Function
    {
        public override BasicType FunctionType => BasicType.Expression;

        public LambdaFunction(string content) : base(content)
        {

        }
    }
}
