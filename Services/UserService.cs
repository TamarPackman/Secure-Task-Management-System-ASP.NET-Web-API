
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;
using Project.interfaces;
using Project.Models;
using Project.Services;
using Task= Project.Models.Task;


namespace Project.Services
{
    public class UserService : IUserService
    {
        private List<User> usersList { get; }
        private UpdateJson<User> updateJson;
        private  readonly ITokenService iTokenService;
        private readonly ITaskService iTaskService;
        public UserService(ITokenService iTokenService,ITaskService iTaskService)
        {
            string basePath = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(basePath, "Data", "users.json");
            updateJson = new UpdateJson<User>(filePath);
            usersList = updateJson.GetList();
            Console.WriteLine(usersList);
            this.iTokenService = iTokenService;
            this.iTaskService=iTaskService;
        }
        public List<User> GetAllList()
        {
           
            return usersList;
        }
       
        public User GetUserById(int id)
        { 
         return usersList.FirstOrDefault(user => user.Id == id);
        }
        public void Create(User newUser)
        {   
            int maxId = usersList.Any() ? usersList.Max(p => p.Id) : 0;
            newUser.Id = maxId + 1;
            if(!(newUser.Type.Equals("User")||newUser.Type.Equals("Admin")))
            newUser.Type="User";
            usersList.Add(newUser);
            updateJson.UpdateListInJson(usersList);
        }
        public void Update(int id, User user)
        {
            User oldUser = GetUserById(id);
            oldUser.Name = user.Name;
            oldUser.Password = user.Password;
            oldUser.Type = user.Type;
            updateJson.UpdateListInJson(usersList);
        }
        public void Delete(int id,string type,int userId)
        {
            int index = usersList.IndexOf(GetUserById(id));
            usersList.RemoveAt(index);
            updateJson.UpdateListInJson(usersList);  
          List<Task> userTaskList= iTaskService.GetAllList(type,userId);
          userTaskList.ForEach(j => { if(j.UserId==id) iTaskService.Delete(j);} );

        }
        public User? GetExistUserAfterSignInWithGoogle(User user)
        {
            return usersList.FirstOrDefault(u => u.Email == user.Email );
        }
        public User? GetExistUser(User user)
        {
            
            return usersList.FirstOrDefault(u => u.Name == user.Name && u.Password == user.Password);
        } 
public  string? Login (User existUser)
{
    var claims = new List<Claim>();

    if (existUser.Type.Equals("Admin"))
    {
        claims.Add(new Claim("type", "Admin"));
    }
    else
    {
        claims.Add(new Claim("type", "User"));
    }

    claims.Add(new Claim("UserId", existUser.Id.ToString()));
    var token = iTokenService.GetToken(claims);
    string  generatedToken = iTokenService.WriteToken(token);
    return generatedToken;
}
}
    }
    public static  partial class ServiceHelper
    {
        public static void AddUserService(this IServiceCollection BuilderServices)
        {
            BuilderServices.AddSingleton<IUserService, UserService>();
        }
    }