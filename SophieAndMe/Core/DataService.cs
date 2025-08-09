namespace SophieAndMe.Core;

public class IdCard
{
    public int Number { get; set; }
}

public interface IDataService
{
    List<string> SharedListChapter { get; set; }
    IdCard IdCard { get; set; }
}

public class DataService : IDataService
{
    public List<string> SharedListChapter { get; set; } =  new List<string>();
    public IdCard IdCard { get; set; }  = new IdCard();
}
