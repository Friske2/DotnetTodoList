using Todolist.Dto;

namespace Todolist.Services;

public interface ITodoService
{
    Task<IEnumerable<GetTodoResponse>> GetAllAsync();
    Task<GetTodoResponse?> GetByIdAsync(int id);
    Task<TodoResponse> CreateAsync(CreateTodoRequest request);
    Task<TodoResponse> UpdateAsync(UpdateTodoRequest request);
    Task<TodoResponse> DeleteAsync(int id);
}