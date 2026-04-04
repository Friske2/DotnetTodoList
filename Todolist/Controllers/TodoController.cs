using Microsoft.AspNetCore.Mvc;
using Todolist.Dto;
using Todolist.Exceptions;
using Todolist.Services;

namespace Todolist.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;
    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet("getDateTime")]
    public ActionResult GetDateTime()
    {
        DateTime now = DateTime.UtcNow;
        return Ok(new Dictionary<string, DateTime> { { "now", now } });
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<GetTodoResponse>>> Get()
    {
        var todos = await _todoService.GetAllAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetTodoResponse>> GetById(int id)
    {
        var todo = await _todoService.GetByIdAsync(id);
        if (todo == null)
            throw new NotFoundException("Todo not found", $"No todo found with id: {id}");
        return Ok(todo);
    }

    [HttpPost()]
    public async Task<ActionResult<TodoResponse>> CreateNewTodo(CreateTodoRequest request)
    {
        var response = await _todoService.CreateAsync(request);
        if (response.Id.Equals(0))
            throw new BadRequestException("Failed to create todo", "An error occurred while creating the todo");
        return Ok(response);
    }

    [HttpPut()]
    public async Task<ActionResult<TodoResponse>> UpdateTodo(UpdateTodoRequest request)
    {
        var response = await _todoService.UpdateAsync(request);
        if (response.Id.Equals(0))
            throw new BadRequestException("Failed to update todo", "An error occurred while updating the todo");
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TodoResponse>> DeleteTodo(int id)
    {
        var response = await _todoService.DeleteAsync(id);
        if (response.Id.Equals(0))
            throw new NotFoundException("Todo not found", $"No todo found with id: {id}");
        return Ok(response);
    }
}

