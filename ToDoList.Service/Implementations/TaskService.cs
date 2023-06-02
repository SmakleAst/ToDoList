using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoList.DAL.Interfaces;
using ToDoList.Domain.Entity;
using ToDoList.Domain.Enum;
using ToDoList.Domain.Extensions;
using ToDoList.Domain.Filters.Task;
using ToDoList.Domain.Response;
using ToDoList.Domain.ViewModels.Task;
using ToDoList.Service.Interfaces;

namespace ToDoList.Service.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly IBaseRepository<TaskEntity> _taskRepository;
        private ILogger<TaskService> _logger;
        
        public TaskService(IBaseRepository<TaskEntity> taskRepository, ILogger<TaskService> logger) =>
            (_taskRepository, _logger) = (taskRepository, logger);

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

                _logger.LogInformation($"Задача создалась: {task.Id} {task.CreatedTime}");
                return new BaseResponse<TaskEntity>()
                {
                    Description = "Задача создана",
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[TaskService.Create]: {exception.Message}");
                return new BaseResponse<TaskEntity>()
                {
                    Description = $"{exception.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> EndTask(long id)
        {
            try
            {
                _logger.LogInformation($"Id задачи на удаление - {id}");
                var task = await _taskRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (task == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Задача не найдена",
                        StatusCode = StatusCode.TaskNotFound
                    };
                }

                task.IsCompleted = true;

                await _taskRepository.Update(task);

                return new BaseResponse<bool>()
                {
                    Description = "Задача завершена",
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[TaskService.Create]: {exception.Message}");
                return new BaseResponse<bool>()
                {
                    Description = $"{exception.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetTasks(TaskFilter filter)
        {
            try
            {
                

                var tasks = await _taskRepository.GetAll()
                    .Where(x => x.IsCompleted == false)
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.Description),
                        x => x.Description.Contains(filter.Description))
                    .WhereIf(filter.Priority.HasValue, x => x.Priority == filter.Priority)
                    .Select(x => new TaskViewModel()
                    {
                        Id = x.Id,
                        Description = x.Description,
                        IsCompleted = x.IsCompleted == true ? "Выполнена" : "Не выполнена",
                        Priority = x.Priority.GetDisplayName(),
                        CreatedTime = x.CreatedTime.ToLongDateString()
                    })
                    .ToListAsync();

                return new BaseResponse<IEnumerable<TaskViewModel>>()
                {
                    Data = tasks,
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[TaskService.Create]: {exception.Message}");
                return new BaseResponse<IEnumerable<TaskViewModel>>()
                {
                    Description = $"{exception.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
