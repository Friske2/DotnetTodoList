using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Todolist.Dto;
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
        // new utc date in c# 
        DateTime now = DateTime.UtcNow;
        Dictionary<String, DateTime> result = new Dictionary<String, DateTime>();
        result.Add("now", now);
        return Ok(result);
    }

    [HttpGet()]
    public ActionResult<IEnumerable<GetTodoResponse>> Get()
    {
        // call service to get all todos
        var todos = _todoService.GetAllAsync().Result;
        return Ok(todos);
    }
    
    [HttpGet("{id}")]
    public ActionResult<GetTodoResponse> GetById(int id)
    {
        // call service to get todo by id
        var todo = _todoService.GetByIdAsync(id).Result;
        if (todo == null)
        {
            var err = new Error()
            {
                StatusCode = 404,
                Message = "Todo not found",
                Details = "No todo found with the given id",
                Timestamp = DateTime.UtcNow
            };
            return NotFound(err);
        }
        return Ok(todo);
    }

    [HttpPost()]
    public ActionResult<TodoResponse> CreateNewTodo(CreateTodoRequest request)
    {
        // call service to create new todo
        var response = _todoService.CreateAsync(request).Result;
        if (response.Id.Equals(0))
        {
            var err = new Error()
            {
                StatusCode = 400,
                Message = "Failed to create todo",
                Details = "An error occurred while creating the todo",
                Timestamp = DateTime.UtcNow
            };
            return BadRequest(err);
        }   
        return Ok(response);
    }
    
    [HttpPut()]
    public ActionResult UpdateTodo(UpdateTodoRequest request)
    {
        // call service to update todo
        var response = _todoService.UpdateAsync(request).Result;
        if (response.Id.Equals(0))
        {
            var err = new Error()
            {
                StatusCode = 400,
                Message = "Failed to update todo",
                Details = "An error occurred while updating the todo",
                Timestamp = DateTime.UtcNow
            };
            return BadRequest(err);
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTodo(int id)
    {
        // call service to delete todo
        var response = _todoService.DeleteAsync(id).Result;
        if (response.Id.Equals(0))
        {
            var err = new Error()
            {
                StatusCode = 400,
                Message = "Failed to delete todo",
                Details = "An error occurred while deleting the todo",
                Timestamp = DateTime.UtcNow
            };
            return BadRequest(err);
        }
        return Ok(response);
    }
}