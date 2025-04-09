using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.interfaces;
using Project.Models;
using IAuthorizationService = Project.interfaces.IAuthorizationService;
using Task = Project.Models.Task;
namespace Project.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]
public class TaskController : ControllerBase
{
    private ITaskService iTaskService;
    private IUserService iUserService;
    private IAuthorizationService iAuthorizationService;
    public TaskController(ITaskService iTaskService, IAuthorizationService iAuthorizationService,IUserService iUserService)
    {
        this.iTaskService = iTaskService;
        this.iAuthorizationService = iAuthorizationService;
        this.iUserService = iUserService;
    }
    [HttpGet]
    public ActionResult<List<Task>> Get()
    {
        (string type, int userId) = iAuthorizationService.GetUserClaims(User);
        return iTaskService.GetAllList(type, userId);
    }
    [HttpGet("{id}")]
    public ActionResult<Task> Get(int id)
    {
        (string type, int userId) = iAuthorizationService.GetUserClaims(User);
        Task? task = iTaskService.GetTaskById(id);
        if (task == null)
            return BadRequest("Invalid id");
        if (iAuthorizationService.IsAccessDenied(task.UserId, type, userId))
            return Unauthorized("Unauthorized: You don't have permission to perform this action.");
        return task;
    }
    [HttpPost]
    public ActionResult Create(Task newTask)
    {
      if(iUserService.GetAllList().FirstOrDefault(x => x.Id==newTask.UserId)==null) 
      return BadRequest("The specified user does not exist.");
        (string type, int userId) = iAuthorizationService.GetUserClaims(User);
        if (iAuthorizationService.IsAccessDenied(newTask.UserId, type, userId))
            return Unauthorized("Unauthorized: You don't have permission to perform this action.");
        iTaskService.Create(newTask);
        return CreatedAtAction(nameof(Create), new { id = newTask.Id }, newTask);
    }
    [HttpPut("{id}")]
    public ActionResult Update(int id, Task task)
    {
        (string type, int userId) = iAuthorizationService.GetUserClaims(User);
        if (id != task.Id)
            return BadRequest("id mismatch");
        Task? oldTask = iTaskService.GetTaskById(id);
        if (oldTask == null)
            return BadRequest("invalid id");
        if (iAuthorizationService.IsAccessDenied(oldTask.UserId, type, userId))
            return Unauthorized("Unauthorized: You don't have permission to perform this action.");

        iTaskService.Update(oldTask, task);
        return NoContent();
    }

    //ID-פונקציה למחיקת אוביקט לפי 
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        (string type, int userId) = iAuthorizationService.GetUserClaims(User);
        Task? task = iTaskService.GetTaskById(id);
        if (task == null)
            return BadRequest("invalid id");
        if (iAuthorizationService.IsAccessDenied(task.UserId, type, userId))
            return Unauthorized("Unauthorized: You don't have permission to perform this action.");
        iTaskService.Delete(task);
        return NoContent();
    }

}
