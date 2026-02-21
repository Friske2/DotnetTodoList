using Microsoft.Extensions.Configuration;
using Todolist.Repository;
using Xunit.Abstractions;

namespace Todolist.test;

public class TestTodoRepositoryIntegration(ITestOutputHelper testOutputHelper)
    : TestTodoRepository(testOutputHelper, CreateRepository())
{
    private static ITodoRepository CreateRepository()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json")
            .Build();
        return new TodoRepo(configuration);
    }
}