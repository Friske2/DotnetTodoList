using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Todolist.Repository;
using Todolist.test.Mocks;
using Xunit.Abstractions;
using Todolist.Models;
namespace Todolist.test;

public abstract class TestTodoRepository
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ITodoRepository _repository;

    protected TestTodoRepository(ITestOutputHelper testOutputHelper, ITodoRepository repository)
    {
        _testOutputHelper = testOutputHelper;
        _repository = repository;
    }

    [Fact]
    public async Task TestGetAllAsync()
    {
        // assert 
        var customTodo = TodoTestHelper.CreateTestTodo("Custom Title");
        var id = await TodoTestHelper.CreateAndAssertTodoAsync(_repository, _testOutputHelper, customTodo);
        // act
        var todos = await _repository.GetAllAsync();
        var jsonTodo = JsonSerializer.Serialize(todos);
        _testOutputHelper.WriteLine(jsonTodo);
        Assert.NotNull(todos);

        // cleanup
        await TodoTestHelper.DeleteAndAssertTodoAsync(_repository, _testOutputHelper, id);
    }

    [Fact]
    public async Task TestGetByIdAsync()
    {
        // assert
        var customTodo = TodoTestHelper.CreateTestTodo("Custom Title");
        var id = await TodoTestHelper.CreateAndAssertTodoAsync(_repository, _testOutputHelper, customTodo);
        // act 
        var todo = await _repository.GetByIdAsync(id);
        var jsonTodo = JsonSerializer.Serialize(todo);
        _testOutputHelper.WriteLine(jsonTodo);
        Assert.NotNull(todo);
        // cleanup
        await TodoTestHelper.DeleteAndAssertTodoAsync(_repository, _testOutputHelper, id);
    }

    [Fact]
    public async Task TestCreateAsync()
    {
        // assert
        var newTodo = new Todo
        {
            Title = "Test Todo",
            Description = "This is a test todo",
            IsCompleted = false,
            Priority = Priority.Low,
            DueDate = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        // act
        var id = await _repository.CreateAsync(newTodo);
        _testOutputHelper.WriteLine($"Created Todo ID: {id}");
        Assert.True(id > 0);

        // cleanup
        await TodoTestHelper.DeleteAndAssertTodoAsync(_repository, _testOutputHelper, id);
    }

    [Fact]
    public async Task TestUpdateAsync()
    {
        // assert
        var customTodo = TodoTestHelper.CreateTestTodo("Custom Title");
        var id = await TodoTestHelper.CreateAndAssertTodoAsync(_repository, _testOutputHelper, customTodo);

        // act
        var todo = await _repository.GetByIdAsync(id);
        if (todo != null)
        {
            todo.Title = "Updated Test Todo";
            var success = await _repository.UpdateAsync(todo);
            _testOutputHelper.WriteLine($"Update Success: {success}");
            Assert.True(success);
        }
        else
        {
            Assert.Fail("Todo not found");
        }

        // cleanup
        await TodoTestHelper.DeleteAndAssertTodoAsync(_repository, _testOutputHelper, id);
    }

    [Fact]
    public async Task TestDeleteAsync()
    {
        // assert
        var customTodo = TodoTestHelper.CreateTestTodo("Custom Title");
        var id = await TodoTestHelper.CreateAndAssertTodoAsync(_repository, _testOutputHelper, customTodo);

        // act
        var success = await _repository.DeleteAsync(id);
        _testOutputHelper.WriteLine($"Delete Success: {success}");
        Assert.True(success);

    }
}