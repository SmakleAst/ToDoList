namespace ToDoList.Domain.Enum
{
    public enum StatusCode
    {
        TaskAlreadyExists = 1,
        TaskNotFound = 2,

        Ok = 200,
        InternalServerError = 500
    }
}
