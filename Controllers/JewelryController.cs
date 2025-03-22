using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.interfaces;
using Project.Models;
using IAuthorizationService = Project.interfaces.IAuthorizationService;
namespace Project.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]
public class JewelryController : ControllerBase
{
    private IJewelService iJewelService;
    private IUserService iUserService;
    private IAuthorizationService iAuthorizationService;
    public JewelryController(IJewelService iJewelService, IAuthorizationService iAuthorizationService,IUserService iUserService)
    {
        this.iJewelService = iJewelService;
        this.iAuthorizationService = iAuthorizationService;
        this.iUserService = iUserService;
    }
    [HttpGet]
    public ActionResult<List<Jewel>> Get()
    {
        (string type, int userId) = iAuthorizationService.GetUserClaims(User);
        return iJewelService.GetAllList(type, userId);
    }
    [HttpGet("{id}")]
    public ActionResult<Jewel> Get(int id)
    {
        (string type, int userId) = iAuthorizationService.GetUserClaims(User);
        Jewel? jewel = iJewelService.GetJewelById(id);
        if (jewel == null)
            return BadRequest("Invalid id");
        if (iAuthorizationService.IsAccessDenied(jewel.UserId, type, userId))
            return Unauthorized("Unauthorized: You don't have permission to perform this action.");
        return jewel;
    }
    [HttpPost]
    public ActionResult Create(Jewel newJewel)
    {
      if(iUserService.GetAllList().FirstOrDefault(x => x.Id==newJewel.UserId)==null) 
      return BadRequest("The specified user does not exist.");
        (string type, int userId) = iAuthorizationService.GetUserClaims(User);
        if (iAuthorizationService.IsAccessDenied(newJewel.UserId, type, userId))
            return Unauthorized("Unauthorized: You don't have permission to perform this action.");
        iJewelService.Create(newJewel);
        return CreatedAtAction(nameof(Create), new { id = newJewel.Id }, newJewel);
    }
    [HttpPut("{id}")]
    public ActionResult Update(int id, Jewel jewel)
    {
        (string type, int userId) = iAuthorizationService.GetUserClaims(User);
        if (id != jewel.Id)
            return BadRequest("id mismatch");
        Jewel? oldJewel = iJewelService.GetJewelById(id);
        if (oldJewel == null)
            return BadRequest("invalid id");
        if (iAuthorizationService.IsAccessDenied(oldJewel.UserId, type, userId))
            return Unauthorized("Unauthorized: You don't have permission to perform this action.");

        iJewelService.Update(oldJewel, jewel);
        return NoContent();
    }

    //ID-פונקציה למחיקת אוביקט לפי 
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        (string type, int userId) = iAuthorizationService.GetUserClaims(User);
        Jewel? jewel = iJewelService.GetJewelById(id);
        if (jewel == null)
            return BadRequest("invalid id");
        if (iAuthorizationService.IsAccessDenied(jewel.UserId, type, userId))
            return Unauthorized("Unauthorized: You don't have permission to perform this action.");
        iJewelService.Delete(jewel);
        return NoContent();
    }

}
