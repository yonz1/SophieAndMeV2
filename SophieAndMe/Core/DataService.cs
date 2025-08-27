namespace SophieAndMe.Core;



public class User 
{
   public string Email { get; set; }
   public string photo { get; set; }
   public string Username { get; set; }
}


public class IdCard
{
    public int Number { get; set; }
}

public class IQuizzId
{
    public string Name { get; set; } = "";
    public string Matier { get; set; } = "";
    public string options { get; set; } = "";
    public bool IsAll { get; set; } = false;
}


public interface IDataService
{
    List<string> SharedListChapter { get; set; }

    User CurrentUser { get; set; }

    IdCard IdCard { get; set; }
    IQuizzId QuizzId { get; set; }
    bool WinBin { get; set; }
    bool IsDark {get; set;}

}

public class DataService : IDataService
{

    public List<string> SharedListChapter { get; set; } =  new ();
    public User CurrentUser { get; set; } = new();
    public IdCard IdCard { get; set; }  = new IdCard();
    public IQuizzId QuizzId { get; set; }  = new IQuizzId();
    public bool WinBin { get; set; } = false;

    public bool IsDark { get; set; } = true;
}
