
using Project.Models;

namespace Project.interfaces
{
    public interface IJewelService
    {
        List<Jewel> GetAllList(string type,int userId);

        Jewel? GetJewelById(int id);

        void Create(Jewel newJewel);

        void Update( Jewel oldJewel, Jewel newJewel );

        void Delete(Jewel jewel);


    }
}
