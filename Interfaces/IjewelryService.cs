using project.Models;

namespace project.interfaces 
{
  public interface IjewerlyService{
    List<Jewel> Get();
    Jewel Get(int id);
    void create(Jewel newJewel);
    void  Update(int id, Jewel newJewel);
    void  Delete(int id);

}  
}
