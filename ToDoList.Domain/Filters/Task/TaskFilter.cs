using ToDoList.Domain.Enum;

namespace ToDoList.Domain.Filters.Task
{
    public class TaskFilter
    {
        public string Description { get; set; }
        public Priority? Priority { get; set; }
    }
}
