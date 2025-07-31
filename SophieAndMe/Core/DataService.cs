namespace SophieAndMe.Core;

public interface IDataService
{
    List<string> SharedListChapter { get; set; }
}

public class DataService : IDataService
{
    public List<string> SharedListChapter { get; set; } =  new List<string>();
}
