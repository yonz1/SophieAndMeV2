using System.Data.SQLite;

namespace SophieAndMe.MVVM.Model;

public class ShortSelect
{
    private static readonly string UserSource = "Data Source=..//..//..//Database//user_value.db";
    private static readonly string ConSource = "Data Source=..//..//..//Database//data_restored.db";
    private static readonly string Tempsource = "Data Source=..//..//..//Database//PublicDB.db";
    private static readonly string ProgressSource = "Data Source=..//..//..//Database//data_progressif.db";
    private static readonly string EDTSource = "Data Source=..//..//..//Database//EDT.db";

    public static string Select(string query,string db)
    {
        using SQLiteConnection c = new SQLiteConnection(db);
        c.Open();
        SQLiteCommand command;
        command = new SQLiteCommand(query, c);
        Console.WriteLine(query);
        string info = "";
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                info = reader.GetString(0);
                Console.WriteLine("Info" + info);
            }
        }

        return info;
    }
}