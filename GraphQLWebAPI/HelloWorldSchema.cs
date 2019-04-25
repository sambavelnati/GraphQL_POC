using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLWebAPI
{
    public class HelloWorldSchema : Schema
    {
        public HelloWorldSchema(HelloWorld query)
        {
            Query = query;
        }
    }
}
