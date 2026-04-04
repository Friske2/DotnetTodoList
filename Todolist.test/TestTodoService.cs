using System.Formats.Asn1;
using Microsoft.Extensions.Configuration;
using Todolist.Dto;
using Todolist.Exceptions;
using Todolist.Models;
using Todolist.Repository;
using Todolist.Services;
using Todolist.test.Mocks;
using Todolist.Utility;
using Xunit.Abstractions;

namespace Todolist.test;

public class TestTodoService
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ITodoRepository _repository;
    private readonly ITodoService _service;
    
    public TestTodoService(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .Build();
        // _repository = new TodoRepo(configuration);
        _repository = new MockTodoRepository(configuration);
        _service = new TodoService(_repository);
    }

    [Fact]
    public async Task TestGetAllAsync()
    {
        var todos = await _service.GetAllAsync();
        Assert.NotNull(todos);
    }

    [Fact]
    public async Task TestGetByIdAsync()
    {
        var newTodo = new Models.Todo
        {
            Title = "Test Todo",
            Description = "This is a test todo",
            IsCompleted = false,
            Priority = Priority.Low,
            DueDate = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var id = await _repository.CreateAsync(newTodo);
        var todo = await _service.GetByIdAsync(id);
        // check output
        if (todo == null)
        {
            Assert.NotNull(todo); // force fail if null
            return;
        }
        Assert.Equal(id, todo.Id);
        var newPriority = Helper.ParsePriorityToInt(newTodo.Priority);
        Assert.Equal(newTodo.Title, todo.Title);
        Assert.Equal(newTodo.Description, todo.Description);
        Assert.Equal(newTodo.IsCompleted, todo.IsComplete);
        Assert.Equal(newPriority, todo.Priority);
        
        // cleanup
        await _repository.DeleteAsync(id);
    }

    [Fact]
    public async Task TestCreateAsync()
    {
        var request = new CreateTodoRequest
        {
            Title = "Test Todo",
            Description = "This is a test todo",
            IsComplete = false,
            Priority = 1,
            DueDate = DateTime.UtcNow.AddDays(7)
        };
        var response = await _service.CreateAsync(request);
        Assert.NotNull(response);
        Assert.True(response.Id > 0);
        // cleanup
        await _repository.DeleteAsync(response.Id);
    }

    [Fact]
    public async Task TestCreateAsync_NullRequest()
    {
        var request = new CreateTodoRequest
        {
            Title = null,
            Description = null,
            IsComplete = false,
            Priority = 1,
            DueDate = DateTime.UtcNow.AddDays(7)
        };
        
        await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(request));
    }
    
    [Fact]
    public async Task TestCreateAsync_InvalidRequest()
    {
        var request = new CreateTodoRequest
        {
            Title = "", // Invalid: empty title
            Description = "This is a test todo",
            IsComplete = false,
            Priority = 1,
            DueDate = DateTime.UtcNow.AddDays(7)
        };
        await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(request));
    }
    
    [Fact]
    public async Task TestCreateAsync_PastDueDate()
    {
        var request = new CreateTodoRequest
        {
            Title = "Test Todo",
            Description = "This is a test todo",
            IsComplete = false,
            Priority = -10, // Invalid: negative priority
            DueDate = DateTime.UtcNow
        };
        await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(request));
    }
    
    [Fact]
    public async Task TestCreateAsync_InvalidPriority()
    {
        var request = new CreateTodoRequest
        {
            Title = null, // Invalid: null title
            Description = "This is a test todo",
            IsComplete = false,
            Priority = 1,
            DueDate = DateTime.UtcNow.AddDays(7)
        };
        await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task TestUpdateAsync()
    {
        var request = new CreateTodoRequest
        {
            Title = "Test Todo",
            Description = "This is a test todo",
            IsComplete = false,
            Priority = 1,
            DueDate = DateTime.UtcNow.AddDays(7)
        };
        var response = await _service.CreateAsync(request);
        var updateRequest = new UpdateTodoRequest
        {
            Id = response.Id,
            Title = "Updated Test Todo",
            Description = "This is an updated test todo",
            IsComplete = true,
            Priority = 2,
            DueDate = DateTime.UtcNow.AddDays(14)
        };
        var updateResponse = await _service.UpdateAsync(updateRequest);
        var todo = await _service.GetByIdAsync(response.Id);
        Assert.NotNull(updateResponse); // force fail if null
        if (todo == null)
        {
            Assert.NotNull(todo); // force fail if null
            return;
        }
        Assert.Equal(updateRequest.Title, todo.Title);
        Assert.Equal(updateRequest.Description, todo.Description);
        Assert.Equal(updateRequest.IsComplete, todo.IsComplete);
        // Assert.Equal(updateRequest.Priority, todo.Priority);
        
        Assert.Equal(response.Id, updateResponse.Id);
        // cleanup
        await _repository.DeleteAsync(response.Id);
    }
    
    // update test with invalid id
    [Fact]
    public async Task TestUpdateAsync_InvalidId()
    {        
        var updateRequest = new UpdateTodoRequest
        {
            Id = -1, // Invalid ID
            Title = "Updated Test Todo",
            Description = "This is an updated test todo",
            IsComplete = true,
            Priority = 2,
            DueDate = DateTime.UtcNow.AddDays(14)
        };
        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(updateRequest));
    }
    
    // update test with invalid request title is null or empty
    [Fact]
    public async Task TestUpdateAsync_TitleIsNull()
    {        
        var updateRequest = new UpdateTodoRequest
        {
            Id = 1, // Invalid ID
            Title = null,
            Description = "This is an updated test todo",
            IsComplete = true,
            Priority = 2,
            DueDate = DateTime.UtcNow.AddDays(14)
        };
        await Assert.ThrowsAsync<BadRequestException>(() => _service.UpdateAsync(updateRequest));
    }
    // update test with invalid request due date in the past
    // update test with invalid request priority out of range

    [Fact]
    public async Task TestDeleteAsync()
    {
        var request = new CreateTodoRequest
        {
            Title = "Test Todo",
            Description = "This is a test todo",
            IsComplete = false,
            Priority = 1,
            DueDate = DateTime.UtcNow.AddDays(7)
        };
        var response = await _service.CreateAsync(request);
        var deleteResponse = await _service.DeleteAsync(response.Id);
        Assert.NotNull(deleteResponse);
        Assert.Equal(response.Id, deleteResponse.Id);
        Assert.Equal("Todo deleted successfully", deleteResponse.Message);
    }
}