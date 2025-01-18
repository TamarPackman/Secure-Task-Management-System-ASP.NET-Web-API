using Microsoft.AspNetCore.Mvc;
using project.interfaces;
using project.Models;
using project.Services;
using project.Exceptions;
namespace project.Controllers;
[ApiController]
[Route("[controller]")]
public class jewerlyController : ControllerBase
{
    IjewerlyService ijewerlyService;
    public jewerlyController(IjewerlyService ijewerlyService)
    {
this.ijewerlyService=ijewerlyService;
    }
    [HttpGet]
    public ActionResult<List<Jewel>> Get()//פונקציה לקבלת הנתונים 
    {
        return ijewerlyService.Get();
    }
   
    [HttpGet("{id}")]
    public ActionResult<Jewel> Get(int id)//ID פונקציה לקבלת אוביקט לפי  
    {
        Jewel jewel= ijewerlyService.Get(id);
       if(jewel==null) 
    {
         throw new NotFoundIdException($"jewel with ID {id} not found."); 
    }
       return jewel;
    }
    
    [HttpPost]
    public ActionResult Insert(Jewel newJewel)//מכניס אוביקט מסוים
    {        
        ijewerlyService.create(newJewel);

        return CreatedAtAction(nameof(Insert), new { id = newJewel.Id }, newJewel);
    }  

    [HttpPut("{id}")]//מעדכן אוביקט מסוים
    public ActionResult Update(int id, Jewel newJewel)
    { 
        if (id != newJewel.Id)
          throw new IdMismatchException("id mismatch");
         
      Jewel?  oldJewel = ijewerlyService.Get(id);
        if (oldJewel == null) 
             throw new NotFoundIdException($"jewel with ID {id} not found."); 
        
       ijewerlyService.Update(id, newJewel);
      return NoContent();

    }
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)// ID מוחק אוביקט לפי 
    {
 Jewel? jewel= ijewerlyService.Get(id);
if(jewel == null)
  throw new NotFoundIdException($"jewel with ID {id} not found."); 
 ijewerlyService.Delete(id);
 return NoContent();
}
}
