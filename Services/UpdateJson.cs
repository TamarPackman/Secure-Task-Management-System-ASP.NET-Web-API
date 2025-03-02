using Project.Models;
using System.Text.Json;
// using System.IO;
namespace Project.Services;

public class UpdateJson<T>
{
    public string JsonPath { get; set; }

    public UpdateJson(string jsonPath)
    {
        JsonPath = jsonPath;
    }

    public List<T> GetList()
    {
        string? jsonContent = File.ReadAllText(JsonPath);

        // אם התוכן ריק או null, מחזירים רשימה ריקה
        if (string.IsNullOrWhiteSpace(jsonContent))
        {
            return new List<T>();
        }

        // אם התוכן לא ריק, מבצעים Deserialize
        return JsonSerializer.Deserialize<List<T>>(jsonContent) ?? new List<T>();
    }


    public void UpdateListInJson(List<T> list)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(list, options);
        File.WriteAllText(JsonPath, json);
    }

}