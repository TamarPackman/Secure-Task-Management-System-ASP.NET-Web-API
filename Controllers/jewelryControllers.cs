using Microsoft.AspNetCore.Mvc;
using project.Models;
namespace project.Controllers;
[ApiController]
[Route("[controller]")]
public class jewerlyController : ControllerBase
{
    private static List<Jewel> jewerlyList;
    static jewerlyController()//בנאי סטטי שנקרא בפעם הראשונה שהמחלקה נטענת
    {
        jewerlyList = new List<Jewel> 
        {
            new Jewel { Id = 1, Name = "עגילי חישוק כסף שיבוץ פאווה" ,Category=CategoryJewel.EARRINGS,Price=425},
            new Jewel { Id = 2, Name = "טבעת כסף משאלת הנסיכה" ,Category=CategoryJewel.RING,Price=379 }
        };
    }

    [HttpGet]
    public IEnumerable<Jewel> Get()//פונקציה לקבלת הנתונים 
    {
        return jewerlyList;
    }
    private Jewel searchJewelById(int id){   //ID פונקציה שמחזירה אוביקט קיים לפי 
    Jewel? jewel = jewerlyList.FirstOrDefault(j => j.Id == id);
        return jewel;
    }
    [HttpGet("{id}")]
    public ActionResult<Jewel> Get(int id)//ID פונקציה לקבלת אוביקט לפי  
    {
        Jewel jewel= searchJewelById(id);
       if(jewel==null) 
       return BadRequest("invalid id");
       return jewel;
    }
    

    [HttpPost]
    public ActionResult Insert(Jewel newJewel)//מכניס אוביקט מסוים
    {        
        int maxId = jewerlyList.Max(j => j.Id);
        newJewel.Id = maxId + 1;
        jewerlyList.Add(newJewel);

        return CreatedAtAction(nameof(Insert), new { id = newJewel.Id }, newJewel);
    }  

    
    [HttpPut("{id}")]//מעדכן אוביקט מסוים
    public ActionResult Update(int id, Jewel newJewel)
    { 
        Jewel? oldJewel = searchJewelById(id);
        if (oldJewel == null) 
            return BadRequest("invalid id");
        if (oldJewel.Id != newJewel.Id)
            return BadRequest("id mismatch");

        oldJewel.Name = newJewel.Name;
        oldJewel.Price = newJewel.Price;
        oldJewel.Category=newJewel.Category;
     return NoContent();

    }
    [HttpDelete]
    public ActionResult Delete(int id)// ID מוחק אוביקט לפי 
    {
        
Jewel? jewel= searchJewelById(id);
if(jewel == null)
return BadRequest("invalid id");
 int index=jewerlyList.IndexOf(jewel);
 jewerlyList.RemoveAt(index);
 return NoContent();
}
}
