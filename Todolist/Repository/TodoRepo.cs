using Todolist.Data;
using Todolist.Exceptions;
using Todolist.Models;

namespace Todolist.Repository;

public class TodoRepo : ITodoRepository
{
    private readonly DataContextDapper _dbContext;
    public TodoRepo(IConfiguration configuration)
    {
        _dbContext = new DataContextDapper(configuration);
    }
    public Task<IEnumerable<Todo>> GetAllAsync()
    {
        var sql = @"select id, title, description, iscompleted, priority, duedate, createdat, updatedat, deletedat from todo";
        var result = _dbContext.Query<Todo>(sql);
        // throw exception is not connected to database
        
        return Task.FromResult(result);
    }

    public Task<Todo?> GetByIdAsync(int id)
    {
        var sql = @"select id, title, description, iscompleted, priority, duedate, createdat, updatedat, deletedat from todo where id = @Id";
        var result = _dbContext.QuerySingle<Todo>(sql, new { Id = id });
        return Task.FromResult(result);
    }

    public Task<int> CreateAsync(Todo todo)
    {
        var sql = @"
            INSERT INTO Todo (title, description, iscompleted, priority, duedate, createdat)
            VALUES (@Title, @Description, @IsCompleted, @Priority, @DueDate, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int)
        ";
        var id = _dbContext.QuerySingle<int>(sql, todo);
        return Task.FromResult(id);
    }

    public Task<bool> UpdateAsync(Todo todo)
    {
        var sql = @"
            UPDATE Todo
            SET title = @Title,
                description = @Description,
                iscompleted = @IsCompleted,
                priority = @Priority,
                duedate = @DueDate,
                updatedat = @UpdatedAt
            WHERE id = @Id
        ";
        
        var success = _dbContext.Execute(sql, todo);
        // check row affected, if 0 means update failed (not found), if > 0 means success
        if(success == false)
        {
            throw new NotFoundException("Todo not found", $"No todo found with id: {todo.Id}");
        }
        return Task.FromResult(success);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var sql = @"
            delete from Todo
            where id = @Id
        ";
        var success = _dbContext.Execute(sql, new { Id = id });
        return Task.FromResult(success);
    }
}