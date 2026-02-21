using Todolist.Repository;
using Xunit.Abstractions;

namespace Todolist.test;

public class TodoTestHelper
{
    public static Models.Todo CreateTestTodo(string title = "Test Todo")
    {
        return new Models.Todo
        {
            Title = title,
            Description = "This is a test todo",
            IsCompleted = false,
            Priority = 1,
            DueDate = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static async Task<int> CreateAndAssertTodoAsync(
        ITodoRepository repository, 
        ITestOutputHelper output,
        Models.Todo todo = null)
    {
        todo ??= CreateTestTodo();
        var id = await repository.CreateAsync(todo);
        output.WriteLine($"Created Todo ID: {id}");
        Assert.True(id > 0);
        return id;
    }
    
    public static async Task<int> DeleteAndAssertTodoAsync(
        ITodoRepository repository, 
        ITestOutputHelper output,
        int id)
    {
        var success = await repository.DeleteAsync(id);
        output.WriteLine($"Delete Success: {success}");
        Assert.True(success);
        return id;
    }
}
