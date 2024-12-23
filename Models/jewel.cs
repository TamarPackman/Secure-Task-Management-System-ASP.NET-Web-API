namespace project.Models;

public enum CategoryJewel{
    RING,
    EARRINGS,
    BRACELET,
    NECKLACE


}
public class Jewel
{
    
    public int Id { get; set; }
    public string? Name { get; set; }
    public CategoryJewel Category { get; set; }
    public double Price { get; set; }

}
