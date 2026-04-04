namespace Todolist.Utility;
using Todolist.Exceptions;
using Todolist.Models;
public static class Helper
{
    // parse int to enum Priority
    public static Priority ParseIntToPriority(int? priority)
    {
        if (priority == null)
        {
            throw new BadRequestException($"Priority is required");
        }
        if (!Enum.IsDefined(typeof(Priority), priority))
        {
            throw new BadRequestException($"Invalid priority value: {priority}. Valid values: 1 (Low), 2 (Medium), 3 (High)");
        }

        return (Priority)priority;
    }
    
    public static int ParsePriorityToInt(Priority priority)
    {
        if (!Enum.IsDefined(typeof(Priority), priority))
        {
            throw new BadRequestException($"Invalid priority value: {priority}");
        }
        return (int)priority;
    }
}