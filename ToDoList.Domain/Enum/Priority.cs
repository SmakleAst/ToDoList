using System.ComponentModel.DataAnnotations;

namespace ToDoList.Domain.Enum
{
    public enum Priority
    {
        [Display(Name = "Не важно")]
        NotImportant = 1,
        [Display(Name = "Важно")]
        Important = 2,
        [Display(Name = "Очень важно")]
        VeryImportant = 3
    }
}
