
using project.interfaces;
using project.Models;

namespace  project.Services
{
public   class jewelryService:IjewerlyService{
 private  List<Jewel> jewerlyList{get;set;}
 

    public jewelryService()//בנאי סטטי שנקרא בפעם הראשונה שהמחלקה נטענת
    {
        jewerlyList = new List<Jewel> 
        {
            new() { Id = 1, Name = "עגילי חישוק כסף שיבוץ פאווה" ,Category=CategoryJewel.EARRINGS,Price=425},
            new() { Id = 2, Name = "טבעת כסף משאלת הנסיכה" ,Category=CategoryJewel.RING,Price=379 }
        };
    }
   public  List<Jewel> Get()=>//פונקציה לקבלת הנתונים 
    
        jewerlyList;
 
    public  Jewel Get(int id)=>jewerlyList.FirstOrDefault(j => j.Id==id);
       
    public  void  create(Jewel newJewel)//מכניס אוביקט מסוים
    {        
        int maxId = jewerlyList.Max(j => j.Id);
        newJewel.Id = maxId + 1;
        jewerlyList.Add(newJewel);

    }  
     public  void  Update(int id, Jewel newJewel)
    { 
        Jewel? oldJewel = Get(id);
        if (oldJewel != null) 
           {
        oldJewel.Name = newJewel.Name;
        oldJewel.Price = newJewel.Price;
        oldJewel.Category=newJewel.Category;
           }
    }
     public  void  Delete(int id)// ID מוחק אוביקט לפי 
    {
        
Jewel? jewel= Get(id);
if(jewel != null)
{
 int index=jewerlyList.IndexOf(jewel);
 jewerlyList.RemoveAt(index);
}
}
 }
 public static class jewelryServiceHelper{
    public static void AddJewelService(this IServiceCollection builderService)
    {
        builderService.AddSingleton<IjewerlyService, jewelryService>();
    }
 }
 
}

 