using Google.Apis.Auth;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.interfaces;
using Project.Models;
using IAuthorizationService = Project.interfaces.IAuthorizationService;

namespace Project.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController(IUserService iUserService, IAuthorizationService iAuthorizationService) : ControllerBase
{  
    private IUserService iUserService = iUserService;
    private IAuthorizationService iAuthorizationService=iAuthorizationService;
     [AllowAnonymous]
    [HttpGet("client-id")]
   
    public IActionResult GetClientId()
    {
        Console.WriteLine("GetClientId called");
        var clientId = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_CLIENT_ID");
        return Ok(new { clientId });
    }
    [HttpGet]
    [Authorize (Policy="Admin")]
    public ActionResult<List<User>> Get()
    {  
     return iUserService.GetAllList();
    }
    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<User> Get(int id)
    {
        (string type,int userId)=iAuthorizationService.GetUserClaims(User);
        if (iAuthorizationService.IsAccessDenied(id,type,userId))
                return Unauthorized("Unauthorized: You don't have permission to perform this action.");
        User? user = iUserService.GetUserById(id);
        if (user == null)
            return BadRequest("Invalid id");
        return user;
    }
  
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public ActionResult Create( User newUser)
    {
     (string type,int userId)=iAuthorizationService.GetUserClaims(User);
        if (newUser.Id == userId)
        {
            return Unauthorized("Unauthorized: You don't have permission to perform this action.");
        }
        iUserService.Create(newUser);
        return CreatedAtAction(nameof(Create), new { id = newUser.Id }, newUser);
    }
   
    [HttpPut("{id}")]
    [Authorize]
    public ActionResult Update(int id,  User newUser)
    {
        if (id != newUser.Id)
            return BadRequest("Id mismatch");
        (string type,int userId)=iAuthorizationService.GetUserClaims(User);
        if (iAuthorizationService.IsAccessDenied(id,type,userId)|| type.Equals("User")&&newUser.Type.Equals("Admin"))
        {
            return Unauthorized("Unauthorized: You don't have permission to perform this action.");
        }
        User? oldUser = iUserService.GetUserById(id);
        if (oldUser == null)
            return BadRequest("Invalid id");
        iUserService.Update(id, newUser);
        return NoContent();
    }
   
    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public ActionResult Delete(int id)
    {
         (string type,int userId)=iAuthorizationService.GetUserClaims(User);
        User? userForDelete = iUserService.GetUserById(id);
        if (userForDelete == null)
            return BadRequest("Invalid id");
        iUserService.Delete(id,type,userId);
        return NoContent();
    }
[AllowAnonymous]
[HttpPost("login")]
public ActionResult Login([FromBody] User user)
{
 User? existUser =iUserService.GetExistUser(user);
    if (existUser == null)
      return  Unauthorized("Unauthorized: You don't have permission to perform this action.");
     string? generatedToken=iUserService.Login(existUser);

    if (string.IsNullOrEmpty(generatedToken ))
    {
        return StatusCode(500, "Error generating token");
    } 
    return Ok(new { Name = existUser.Name, token = generatedToken });
}
[AllowAnonymous]
 [HttpPost("google")]
 
    public async Task<IActionResult> VerifyGoogleToken([FromBody] GoogleTokenModel model)
    {

            var payload = await GoogleJsonWebSignature.ValidateAsync(model.Token);
             User? existUser = iUserService.GetExistUserAfterSignInWithGoogle(new User() { Email = payload.Email });
              if (existUser == null)
      return  Unauthorized("Unauthorized: You don't have permission to perform this action.");
 string? generatedToken=iUserService.Login(existUser);
  if (string.IsNullOrEmpty(generatedToken ))
    {
        return StatusCode(500, "Error generating token");
    } 
            return Ok(new { Name = existUser.Name, token = generatedToken  });       
                  
    }
    
}

public class GoogleTokenModel
{
    public string Token { get; set; }
}



