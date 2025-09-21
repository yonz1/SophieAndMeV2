namespace SophieAndMe.Core;

public class IdCard
{
    public int Number { get; set; }
}

public class IQuizzId
{
    public string old { get; set; } = "";
    public string old_quest { get; set; } = "";
    public string Nameindex { get; set; } = "";
    public string Name { get; set; } = "";
    public string Matier { get; set; } = "";
    public string options { get; set; } = "";
    public bool IsAll { get; set; } = false;
}

public interface IDataService
{
    List<string> SharedListChapter { get; set; }
    IdCard IdCard { get; set; }
    IQuizzId QuizzId { get; set; }
    bool WinBin { get; set; }
    bool IsDark {get; set;}
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}

public class DataService : IDataService
{
    public List<string> SharedListChapter { get; set; } =  new List<string>();
    public IdCard IdCard { get; set; }  = new IdCard();
    public IQuizzId QuizzId { get; set; }  = new IQuizzId();
    public bool WinBin { get; set; } = false;
    public bool IsDark { get; set; } = true;
    public DateTime Start { get; set; } = DateTime.Now;
    public DateTime End { get; set; }
    
}
