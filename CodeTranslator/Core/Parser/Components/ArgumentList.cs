using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeTranslator.Core.Parser.Components
{
    public class ArgumentList : IEnumerable<Argument>
    {
        private readonly List<Argument> _argumentList;

        public IList<Argument> Arguments => _argumentList;

        public ArgumentList(params Argument[] args)
        {
            _argumentList = args.ToList();
        }

        internal void Add(Argument arg) => _argumentList.Add(arg);

        internal void Remove(Argument arg) => _argumentList.Remove(arg);

        public IEnumerator<Argument> GetEnumerator() => _argumentList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _argumentList.GetEnumerator();
    }
}
