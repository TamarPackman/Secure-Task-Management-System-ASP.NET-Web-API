namespace Project.Models;

public enum CategoryJewel
{
    Ring,
    Earrings,
    Necklace,
    Bracelet
}

public class Jewel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public CategoryJewel Category { get; set; } 
}



