
using Project.Models;
using Task = Project.Models.Task;

namespace Project.interfaces
{
    public interface ITaskService
    {
        List<Task> GetAllList(string type,int userId);

        Task? GetTaskById(int id);

        void Create(Task newTask);

        void Update( Task oldTask, Task newTask );

        void Delete(Task task);


    }
}
