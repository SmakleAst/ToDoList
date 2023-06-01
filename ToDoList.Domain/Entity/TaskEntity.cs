using ToDoList.Domain.Enum;

namespace ToDoList.Domain.Entity
{
    public class TaskEntity
    {
        public long Id { get; set; }
        public bool IsCompleted { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public Priority Priority { get; set; }
    }
}
