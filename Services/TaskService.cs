
using System.Reflection.Metadata.Ecma335;
using Project.interfaces;
using Project.Models;
using Task = Project.Models.Task;


namespace Project.Services
{
    public class TaskService : ITaskService
    {
        private List<Task> TaskList { get; set; }
        private UpdateJson<Task> UpdateJson { get; set; }
        public TaskService()
        {
            string basePath = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(basePath, "Data", "Task.json");
            UpdateJson = new UpdateJson<Task>(filePath);
            TaskList = UpdateJson.GetList();
        }
        public List<Task> GetAllList(string type, int userId)
        {
            if (type.Equals("User"))
                return TaskList.Where((task) => task.UserId == userId).ToList();
            else
                return TaskList;
        }

        public Task? GetTaskById(int id)
        {
            return TaskList.FirstOrDefault(p => p.Id == id);
        }
     
        public void Create(Task newTask)
        {
            int maxId = TaskList.Any() ? TaskList.Max(p => p.Id) : 0;
            newTask.Id = maxId + 1;
            TaskList.Add(newTask);
            UpdateJson.UpdateListInJson(TaskList);

        }
      
        public void Update(Task oldTask, Task newTask)
        {

            oldTask.Name = newTask.Name;
            oldTask.Description = newTask.Description;
            oldTask.Status = newTask.Status;
            UpdateJson.UpdateListInJson(TaskList);

        }
        public void Delete(Task task)
        {

            int index = TaskList.IndexOf(task);
            TaskList.RemoveAt(index);
            UpdateJson.UpdateListInJson(TaskList);

        }

    }

    public static partial class ServiceHelper
    {
        public static void AddTaskService(this IServiceCollection BuilderServices)
        {
            BuilderServices.AddSingleton<ITaskService, TaskService>();
        }
    }

}

