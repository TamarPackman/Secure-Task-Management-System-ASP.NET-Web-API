
using System.Reflection.Metadata.Ecma335;
using Project.interfaces;
using Project.Models;


namespace Project.Services
{
    public class JewelService : IJewelService
    {
        private List<Jewel> JewelryList { get; set; }
        private UpdateJson<Jewel> UpdateJson { get; set; }
        public JewelService()
        {
            string basePath = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(basePath, "Data", "jewelry.json");
            UpdateJson = new UpdateJson<Jewel>(filePath);
            JewelryList = UpdateJson.GetList();
        }
        public List<Jewel> GetAllList(string type, int userId)
        {
            if (type.Equals("User"))
                return JewelryList.Where((jewel) => jewel.UserId == userId).ToList();
            else
                return JewelryList;
        }

        public Jewel? GetJewelById(int id)
        {
            return JewelryList.FirstOrDefault(p => p.Id == id);
        }
     
        public void Create(Jewel newJewel)
        {
            int maxId = JewelryList.Any() ? JewelryList.Max(p => p.Id) : 0;
            newJewel.Id = maxId + 1;
            JewelryList.Add(newJewel);
            UpdateJson.UpdateListInJson(JewelryList);

        }
      
        public void Update(Jewel oldJewel, Jewel newJewel)
        {

            oldJewel.Name = newJewel.Name;
            oldJewel.Price = newJewel.Price;
            oldJewel.Category = newJewel.Category;
            UpdateJson.UpdateListInJson(JewelryList);

        }
        public void Delete(Jewel jewel)
        {

            int index = JewelryList.IndexOf(jewel);
            JewelryList.RemoveAt(index);
            UpdateJson.UpdateListInJson(JewelryList);

        }

    }

    public static partial class ServiceHelper
    {
        public static void AddJewelService(this IServiceCollection BuilderServices)
        {
            BuilderServices.AddSingleton<IJewelService, JewelService>();
        }
    }

}

