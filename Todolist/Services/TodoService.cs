using Todolist.Dto;
using Todolist.Exceptions;
using Todolist.Repository;
using Todolist.Models;
using Todolist.Utility;
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
            Priority = Helper.ParsePriorityToInt(todo.Priority),
            DueDate = todo.DueDate,
            CreatedAt = todo.CreatedAt,
            UpdatedAt = todo.UpdatedAt
        });
    }

    public async Task<GetTodoResponse?> GetByIdAsync(int id)
    {
        var todo = await repository.GetByIdAsync(id);
        if (todo == null)
        {
            return null;
        }
        var priority = Helper.ParsePriorityToInt(todo.Priority);
       
        return new GetTodoResponse
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            IsComplete = todo.IsCompleted,
            Priority = priority,
            DueDate = todo.DueDate,
            CreatedAt = todo.CreatedAt,
            UpdatedAt = todo.UpdatedAt
        };
    }

    public async Task<TodoResponse> CreateAsync(CreateTodoRequest request)
    {
        var priority = Helper.ParseIntToPriority(request.Priority);
        if (request.Title == null || request.Title.Trim() == "")
        {
            throw new BadRequestException("Title is required");
        }
        
        if(request.DueDate < DateTime.UtcNow)
        {
            throw new BadRequestException("Due date cannot be in the past");
        }
        
        var todo = new Todo
        {
            Title = request.Title,
            Description = request.Description,
            IsCompleted = request.IsComplete,
            Priority = priority,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var id = await repository.CreateAsync(todo);
        return new TodoResponse
        {
            Id = id,
            Message = "Todo created successfully",
            CreatedAt = todo.CreatedAt,
        };
    }

    public async Task<TodoResponse> UpdateAsync(UpdateTodoRequest request)
    {
        if (request.Title == null || request.Title.Trim() == "")
        {
            throw new BadRequestException("Title is required");
        }

        var priority = Helper.ParseIntToPriority(request.Priority);
        var todo = new Todo
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            IsCompleted = request.IsComplete,
            Priority = priority,
            DueDate = request.DueDate,
            CreatedAt = request.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var result = await repository.UpdateAsync(todo);
        if (!result)
        {
            throw new NotFoundException($"Todo with id {request.Id} not found");
        }
        return new TodoResponse
        {
            Id = request.Id,
            Message = "Todo updated successfully",
            CreatedAt = todo.CreatedAt,
        };
    }

    public async Task<TodoResponse> DeleteAsync(int id)
    {
        var result = await repository.DeleteAsync(id);
        if (!result)
        {
            return new TodoResponse
            {
                Id = id,
                Message = "Failed to delete todo",
                CreatedAt = DateTime.UtcNow,
            };
        }
        return new TodoResponse
        {
            Id = id,
            Message = "Todo deleted successfully",
            CreatedAt = DateTime.UtcNow,
        };
    }
}