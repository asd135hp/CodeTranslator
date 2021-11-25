using System;

namespace CodeTranslator.Core.Translator.Github
{
    public sealed class GithubTranslator : GenericTranslator
    {
        public GithubTranslator(string repoUrl) : this(new Uri(repoUrl))
        {

        }

        public GithubTranslator(Uri uri)
        {
            
        }
    }
}
