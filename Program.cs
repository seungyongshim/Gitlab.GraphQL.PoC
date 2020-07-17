using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gitlab.GraphQL.PoC
{
    public class Response
    {
        public ProjectContent Project { get; set; }
        public class ProjectContent
        {
            public string Name { get; set; }

            public IssuesContent Issues { get; set; }

            public class IssuesContent
            {
                public List<Node> Nodes { get; set; }

                public class Node
                {
                    public string Title { get; set; }
                }
                public PageInfoContent PageInfo { get; set; }

                public class PageInfoContent
                {
                    public string EndCursor { get; set; }
                    public bool HasNextPage { get; set; }
                }
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            using var graphQLClient = new GraphQLHttpClient("http://wish.mirero.co.kr/api/graphql", new SystemTextJsonSerializer());

            var Request = new GraphQLRequest
            {
                Query = @"
                {
                    project(fullPath: ""mirero/team/solution-group/bootcamp"") {
                        name
                        issues(createdAfter: ""2020-07-10T00:00:00"") {
                            nodes {
                                title
                            }
                            pageInfo {
                                endCursor
                                hasNextPage
                            }
                        }
                    }
                }"
            };

            var graphQLResponse = await graphQLClient.SendQueryAsync<Response>(Request);

            Console.WriteLine(JsonSerializer.Serialize(graphQLResponse, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
