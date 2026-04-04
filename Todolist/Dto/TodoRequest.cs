namespace Todolist.Dto;

public class CreateTodoRequest
{
    public String? Title { get; set; } = "";
    public String? Description  { get; set; } = "";
    public Boolean IsComplete  { get; set; } = false;
    public int? Priority  { get; set; } = 0;
    public DateTime DueDate { get; set; } = DateTime.UtcNow;
    public DateTime? CreatedAt  { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class UpdateTodoRequest
{
    public int Id { get; set; } = 0;
    public String Title { get; set; } = "";
    public String Description  { get; set; } = "";
    public Boolean IsComplete  { get; set; } = false;
    public int? Priority  { get; set; } = 0;
    public DateTime DueDate { get; set; } = DateTime.UtcNow;
    public DateTime? CreatedAt  { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}