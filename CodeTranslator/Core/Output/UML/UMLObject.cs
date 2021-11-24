using CodeTranslator.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTranslator.Core.Output.UML
{
    internal abstract class UMLObject : GenericOutput
    {
        public UMLObject(CodeFile codeFile) : base(codeFile)
        {
        }
    }
}
