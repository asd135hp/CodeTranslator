using System;
using System.Collections.Generic;
using System.Text;

using GraphQL;

namespace CodeTranslator.Model.Github
{
    internal class GithubGraphQL
    {
        private static GithubGraphQL _singleton = null;

        public static GithubGraphQL GetInstance()
        {
            if(_singleton == null)
            {
                _singleton = new GithubGraphQL();
            }
            return _singleton;
        }

        private GithubGraphQL()
        {

        }
    }
}
