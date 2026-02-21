namespace Todolist.Dto;

public class TodoResponse
{
    public int Id { get; set; } 
    public String Message { get; set; } = "OK";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class GetTodoResponse
{
    public int Id { get; set; }
    public String Title { get; set; } = "";
    public string? Description  { get; set; } = "";
    public Boolean IsComplete  { get; set; } = false;
    public int Priority  { get; set; } = 0;
    public DateTime DueDate { get; set; } = DateTime.UtcNow;
    public DateTime? CreatedAt  { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}