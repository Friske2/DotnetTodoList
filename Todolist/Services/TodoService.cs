using Todolist.Dto;
using Todolist.Repository;

namespace Todolist.Services;

public class TodoService(ITodoRepository repository) : ITodoService
{
    public async Task<IEnumerable<GetTodoResponse>> GetAllAsync()
    {
        var todos = await repository.GetAllAsync();
        return todos.Select(todo => new GetTodoResponse
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            IsComplete = todo.IsCompleted,
            Priority = todo.Priority,
            DueDate = todo.DueDate,
            CreatedAt = todo.CreatedAt,
            UpdatedAt = todo.UpdatedAt
        });
    }

    public Task<GetTodoResponse?> GetByIdAsync(int id)
    {
        var todo = repository.GetByIdAsync(id).Result;
        if (todo == null)
        {
            return Task.FromResult<GetTodoResponse?>(null);
        }
        return Task.FromResult<GetTodoResponse?>(new GetTodoResponse
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            IsComplete = todo.IsCompleted,
            Priority = todo.Priority,
            DueDate = todo.DueDate,
            CreatedAt = todo.CreatedAt,
            UpdatedAt = todo.UpdatedAt
        });
    }

    public Task<TodoResponse> CreateAsync(CreateTodoRequest request)
    {
        var todo = new Models.Todo
        {
            Title = request.Title,
            Description = request.Description,
            IsCompleted = request.IsComplete,
            Priority = request.Priority,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var id = repository.CreateAsync(todo).Result;
        return Task.FromResult(new TodoResponse
        {
            Id = id,
            Message = "Todo created successfully",
            CreatedAt = todo.CreatedAt,
        });
    }

    public Task<TodoResponse> UpdateAsync(UpdateTodoRequest request)
    {
        var todo = new Models.Todo
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            IsCompleted = request.IsComplete,
            Priority = request.Priority,
            DueDate = request.DueDate,
            CreatedAt = request.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var result = repository.UpdateAsync(todo).Result;
        if (!result) {
            return Task.FromResult(new TodoResponse
            {
                Id = request.Id,
                Message = "Failed to update todo",
                CreatedAt = todo.CreatedAt,
            });
        }
        return Task.FromResult(new TodoResponse
        {
            Id = request.Id,
            Message = "Todo updated successfully",
            CreatedAt = todo.CreatedAt,
        });
    }

    public Task<TodoResponse> DeleteAsync(int id)
    {
        var result = repository.DeleteAsync(id).Result;
        if (!result) {
            return Task.FromResult(new TodoResponse
            {
                Id = id,
                Message = "Failed to delete todo",
                CreatedAt = DateTime.UtcNow,
            });
        }
        return Task.FromResult(new TodoResponse
        {
            Id = id,
            Message = "Todo deleted successfully",
            CreatedAt = DateTime.UtcNow,
        });
    }
}