using Todolist.Models;
using Todolist.Repository;

namespace Todolist.test.Mocks;

public class TodoRepositiory: ITodoRepository
{
    public Task<IEnumerable<Todo>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Todo?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> CreateAsync(Todo todo)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Todo todo)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}