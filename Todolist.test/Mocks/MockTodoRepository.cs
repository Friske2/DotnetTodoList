using Microsoft.Extensions.Configuration;
using Todolist.Models;
using Todolist.Repository;

namespace Todolist.test.Mocks;

public class MockTodoRepository: ITodoRepository
{
    private readonly List<Todo> _todos = new();
    private int _nextId = 1;
    // Constructor 
    public MockTodoRepository(IConfiguration? configuration = null)
    {
        // Not actually used, but declared to match the real repository interface.
    }
    public Task<IEnumerable<Todo>> GetAllAsync()
    {
        return Task.FromResult(_todos.AsEnumerable());
    }

    public Task<Todo?> GetByIdAsync(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(todo);
    }

    public Task<int> CreateAsync(Todo todo)
    {
        todo.Id = _nextId++;
        _todos.Add(todo);
        return Task.FromResult(todo.Id);
    }

    public Task<bool> UpdateAsync(Todo todo)
    {
        var existing = _todos.FirstOrDefault(t => t.Id == todo.Id);
        if (existing == null) return Task.FromResult(false);
        
        existing.Title = todo.Title;
        existing.Description = todo.Description;
        existing.IsCompleted = todo.IsCompleted;
        existing.Priority = todo.Priority;
        existing.DueDate = todo.DueDate;
        existing.UpdatedAt = DateTime.UtcNow;
        
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null) return Task.FromResult(false);
        
        _todos.Remove(todo);
        return Task.FromResult(true);
    }
}