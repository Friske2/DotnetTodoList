using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Todolist.Repository;
using Xunit.Abstractions;

namespace Todolist.test;

public class TestTodoRepositiory
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ITodoRepository _repository;

    public TestTodoRepositiory(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json")
            .Build();
        _repository = new TodoRepo(configuration);

        // var inMemorySetting = new Dictionary<string, string> {
        //     {"ConnectionStrings:DefaultConnection", "Server=localhost;Database=Todolist;User Id=sa;Password=your_password;TrustServerCertificate=True;"}
        // };
        // _configuration = new ConfigurationBuilder()
        //     .AddInMemoryCollection(inMemorySetting)
        //     .Build();
        // _repository = new TodoRepo(_configuration);
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
        var todo = _repository.GetByIdAsync(id);
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
        var newTodo = new Models.Todo
        {
            Title = "Test Todo",
            Description = "This is a test todo",
            IsCompleted = false,
            Priority = 1,
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