using System.ComponentModel.DataAnnotations;

namespace ToDoList.Domain.ViewModels.Task
{
    public class TaskViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Готовность")]
        public string IsCompleted { get; set; }

        [Display(Name = "Важность")]
        public string Priority { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Дата создания")]
        public string CreatedTime { get; set; }
    }
}
