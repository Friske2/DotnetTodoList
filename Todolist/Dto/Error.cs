namespace Todolist.Dto;

public class Error
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; }
}