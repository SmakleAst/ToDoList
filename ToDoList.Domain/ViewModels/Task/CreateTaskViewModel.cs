using ToDoList.Domain.Enum;

namespace ToDoList.Domain.ViewModels.Task
{
    public class CreateTaskViewModel
    {
        public string Description { get; set; }
        public Priority Priority { get; set; }
    }
}
