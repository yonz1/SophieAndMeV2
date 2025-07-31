namespace SophieAndMe.Core;


public class User 
{
   public string Email { get; set; }
   public string photo { get; set; }
   public string Username { get; set; }
}


public interface IDataService
{
    List<string> SharedListChapter { get; set; }
    User CurrentUser { get; set; }
}

public class DataService : IDataService
{
    public List<string> SharedListChapter { get; set; } =  new ();
    public User CurrentUser { get; set; } = new();
}
