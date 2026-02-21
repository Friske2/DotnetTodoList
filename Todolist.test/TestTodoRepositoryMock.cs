using Microsoft.Extensions.Configuration;
using Todolist.Repository;
using Todolist.test.Mocks;
using Xunit.Abstractions;

namespace Todolist.test;

public class TestTodoRepositoryMock(ITestOutputHelper testOutputHelper)
    : TestTodoRepository(testOutputHelper, new MockTodoRepository(null));