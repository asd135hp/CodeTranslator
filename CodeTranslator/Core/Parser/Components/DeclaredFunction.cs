using CodeTranslator.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTranslator.Core.Parser.Components
{
    internal class DeclaredFunction : Function
    {
        public override BasicType FunctionType => BasicType.Statement;

        public DeclaredFunction(string content) : base(content)
        {

        }
    }
}
