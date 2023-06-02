using ToDoList.Domain.Enum;

namespace ToDoList.Domain.ViewModels.Task
{
    public class CreateTaskViewModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                throw new ArgumentNullException(Description, "Укажите описание задачи");
            }
        }
    }
}
