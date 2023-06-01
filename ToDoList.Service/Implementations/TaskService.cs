using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoList.DAL.Interfaces;
using ToDoList.Domain.Entity;
using ToDoList.Domain.Enum;
using ToDoList.Domain.Response;
using ToDoList.Domain.ViewModels.Task;
using ToDoList.Service.Interfaces;

namespace ToDoList.Service.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly IBaseRepository<TaskEntity> _taskRepository;
        private ILogger<TaskService> _logger;
        
        public TaskService(IBaseRepository<TaskEntity> taskEntity, ILogger<TaskService> logger) =>
            (_taskRepository, _logger) = (taskEntity, logger);

        public async Task<IBaseResponse<TaskEntity>> Create(CreateTaskViewModel model)
        {
            try
            {
                model.Validate();

                _logger.LogInformation($"Запрос на создание задачи - {model.Description}");

                var task = await _taskRepository.GetAll()
                    .Where(x => x.CreatedTime.Date == DateTime.Today)
                    .FirstOrDefaultAsync(x => x.Description == model.Description);

                if (task != null)
                {
                    return new BaseResponse<TaskEntity>()
                    {
                        Description = "Такая задача уже есть",
                        StatusCode = StatusCode.TaskAlreadyExists
                    };
                }

                task = new TaskEntity()
                {
                    Description= model.Description,
                    IsCompleted = false,
                    Priority = model.Priority,
                    CreatedTime = DateTime.Now,
                };

                await _taskRepository.Create(task);

                _logger.LogInformation($"Задача создалась: {task.Description} {task.CreatedTime}");
                return new BaseResponse<TaskEntity>()
                {
                    Description = "Задача создана",
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[TaskService.Create]: {exception.Message}");
                return new BaseResponse<TaskEntity>
                {
                    Description = $"{exception.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
