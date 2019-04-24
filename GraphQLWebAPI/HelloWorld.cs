using GraphQL.Types;
namespace GraphQLWebAPI
{
    public class HelloWorld : ObjectGraphType
    {
        public HelloWorld()
        {
            Field<StringGraphType>(
                name: "hello",
                resolve: context => "world"
            );
            Field<StringGraphType>(
                name: "howdy",
                resolve: context => "universe"
            );
        }
    }
}
