using Project.Models;
using System.Text.Json;
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
        if (string.IsNullOrWhiteSpace(jsonContent))
        {
            return new List<T>();
        }
        return JsonSerializer.Deserialize<List<T>>(jsonContent) ?? new List<T>();
    }


    public void UpdateListInJson(List<T> list)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(list, options);
        File.WriteAllText(JsonPath, json);
    }

}