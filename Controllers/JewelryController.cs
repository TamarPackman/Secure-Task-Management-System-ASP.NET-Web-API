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
    private IAuthorizationService iAuthorizationService;
    public JewelryController(IJewelService iJewelService, IAuthorizationService iAuthorizationService)
    {
        this.iJewelService = iJewelService;
        this.iAuthorizationService = iAuthorizationService;
    }
    //פונקציה לקבלת רשימת הנתונים
  
    [HttpGet]
    public ActionResult<List<Jewel>> Get() 
    { 
        (string type,int userId)=iAuthorizationService.GetUserClaims(User);
     return iJewelService.GetAllList(type,userId);
    }

    //id-פונקציה לקבלת אוביקט לפי 
    [HttpGet("{id}")]
    public ActionResult<Jewel> Get(int id)
    {
        //  (string type,int userId)=iAuthorizationService.GetUserClaims(User);
        //   if(iAuthorizationService.IsAccessDenied(id,type,userId))
        // return Unauthorized();
         (string type,int userId)=iAuthorizationService.GetUserClaims(User);
        Jewel? jewel = iJewelService.GetJewelById(id);
        if (jewel == null)
            return BadRequest("Invalid id"); 
          if(iAuthorizationService.IsAccessDenied(jewel.UserId,type,userId))
              return Unauthorized();
        return jewel;
    }
    //מכניס אוביקט חדש לרשימה
    [HttpPost]
    public ActionResult Create(Jewel newJewel)
    {    
        (string type,int userId)=iAuthorizationService.GetUserClaims(User);
         if(iAuthorizationService.IsAccessDenied(newJewel.UserId,type,userId))
        return Unauthorized();
        iJewelService.Create(newJewel);
        return CreatedAtAction(nameof(Create), new { id = newJewel.Id }, newJewel);
    }  
    //מעדכן אוביקט מהרשימה
    [HttpPut("{id}")]
    public ActionResult Update(int id, Jewel jewel)
    {   (string type,int userId)=iAuthorizationService.GetUserClaims(User);
       if (id != jewel.Id)
            return BadRequest("id mismatch");
         Jewel? oldJewel = iJewelService.GetJewelById(id);
        if (oldJewel == null)
            return BadRequest("invalid id");
            if(iAuthorizationService.IsAccessDenied(oldJewel.Id,type,userId))
          return Unauthorized();
      
         iJewelService.Update( oldJewel,jewel);
        return NoContent(); 
    }
    
    //ID-פונקציה למחיקת אוביקט לפי 
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
       (string type,int userId)=iAuthorizationService.GetUserClaims(User);
       Jewel? jewel = iJewelService.GetJewelById( id);
        if (jewel == null)
            return BadRequest("invalid id");
             if(iAuthorizationService.IsAccessDenied(jewel.Id,type,userId))
          return Unauthorized();
         iJewelService.Delete(jewel);
         return NoContent();
    }

}
//לעשות שמנהל אחד לא יוכל לראות גם תכשיטים של מנהל אחר
//לבדוק שמוסיפים תכשיט לUSER שקיים
//כשמוחקים כזה USER למחוק לו את התכשיטים
//לשנות את ה POLICY