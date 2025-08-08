using System.Data.SQLite;
using System.Text;
using System.Windows.Forms;
using SophieAndMe.Core;
using SophieAndMe.MVVM.View;
using SophieAndMe.MVVM.ViewModel;
using Application = System.Windows.Application;


namespace SophieAndMe.MVVM.Model
{
    abstract class DbInteraction
    {

        // ################################################### Initialisations 

        private static readonly List<string> Aquestion = new List<string>();
        private static readonly List<string> Arepnse = new List<string>();
        private static readonly List<string> AurlQuestion = new List<string>();
        private static readonly List<string> AurlRep = new List<string>();
        private static readonly List<string> Name = new List<string>();
        private static readonly List<string> ListMat =
            ["Mathématiques", "Physique", "SI", "Français", "Anglais", "Erreurs"];

        private static readonly string ConSource = "Data Source=..//..//..//Database//data_restored.db";
        private static readonly string Tempsource = "Data Source=..//..//..//Database//PublicDB.db";


        private static readonly List<string> Level = new List<string>();
        private static readonly List<string> Course = new List<string>();
        private static readonly List<string> Question = new List<string>();
        private static readonly List<string> ImageQuestion = new List<string>();
        private static readonly List<string> Reponse = new List<string>();
        private static readonly List<string> ImageRep = new List<string>();
        private static readonly List<string> Difficulty = new List<string>();

        private static readonly List<string> TempList = [];
        
        private static IDataService _dataService;
        // ################################################### Fonctions


        public static void MarkData(string question, string repnse, string urlQuestion, string urlRep)
        {
            using SQLiteConnection c = new SQLiteConnection(ConSource);
            c.Open();
            var query = "SELECT COUNT(*) FROM Marked WHERE  question = \"" + question + "\" AND Matier =   \"" +
                        Application.Current.Properties["matier"]?.ToString() + "\"";
            using SQLiteCommand cmd = new SQLiteCommand(query, c);
            long count = (long)cmd.ExecuteScalar();

            if (count == 0)
            {
                var mat = "\"" + App.Current.Properties["matier"].ToString() + "\",";
                var quest = "\"" + question.Replace("\\/", "/") + "\",";
                var rep = "\"" + repnse + "\",";
                var questionImg = "\"" + urlQuestion + "\",";
                var reponseImg = "\"" + urlRep + "\"";
                query =
                    "INSERT INTO Marked (Matier,question,reponse,image_question_url,image_answer_url) VALUES (" +
                    mat + quest + rep + questionImg + reponseImg + ")";
                using SQLiteCommand insertCmd = new SQLiteCommand(query, c);
                insertCmd.ExecuteNonQuery();
            }
        }

        public static void UnMark(string question)
        {
            using (SQLiteConnection c = new SQLiteConnection(ConSource))
            {
                c.Open();
                string query = "DELETE FROM Marked WHERE REPLACE(question, ' ', '') =  REPLACE(\"" + question +
                               "\", ' ', '')";
                using (SQLiteCommand cmd = new SQLiteCommand(query, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<string> GetName(object? mat)
        {
            var result = new List<string>();
            if ((string)mat! != "All")
            {
                using var db = new SQLiteConnection(ConSource);
                db.Open();
                string query = $"SELECT name FROM [{mat}] ORDER BY name";
                using var cmd = new SQLiteCommand(query, db);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string nom = reader.GetString(0);
                    if (!result.Contains(nom))
                        result.Add(nom);
                }
            }
            else
            {
                result = ListMat;
            }

            return result;
        }

  public static (List<string>, List<string>, List<string>, List<string>) Retrievequizz(
    string? nameindex, string id, MainViewModel mainVm)
{
    var connection = new SQLiteConnection(ConSource);
    var resultsQuestions = new List<string>();
    var resultsReponses = new List<string>();
    var resultsUrlQuestion = new List<string>();
    var resultsUrlRep = new List<string>();
    var resultsName = new List<string>();

    try
    {
        connection.Open();
        SQLiteCommand command;

        if ((string)Application.Current.Properties["matier"]! == "All")
        {
            _dataService = App.DataService;
            var chapter = _dataService.SharedListChapter;
            
            var sb = new StringBuilder();
            var parameters = new List<SQLiteParameter>();

            for (int i = 0; i < chapter.Count; i++)
            {
                if (i > 0) sb.Append(" UNION ");
                sb.Append($"SELECT question, reponse, image_question_url, image_answer_url,name FROM {nameindex} WHERE REPLACE(name, ' ', '') = REPLACE(@name{i}, ' ', '')");
                parameters.Add(new SQLiteParameter($"@name{i}", chapter[i]));
            }
            command = new SQLiteCommand(sb.ToString(), connection);
            command.Parameters.AddRange(parameters.ToArray());
            Application.Current.Properties["matier"] = nameindex;
        }
        else if (nameindex != null && nameindex.Contains("Marked"))
        {
            string query = "SELECT question,reponse,image_question_url,image_answer_url FROM Marked WHERE Matier = @matier";
            command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@matier", Application.Current.Properties["matier"]?.ToString());
        }
        else
        {
            string query = $"SELECT question,reponse,image_question_url,image_answer_url FROM {Application.Current.Properties["matier"]} WHERE name = @name";
            command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@name", nameindex);
        }
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                resultsQuestions.Add(reader.GetString(0));
                resultsReponses.Add(reader.GetString(1));
                resultsUrlQuestion.Add(reader.GetString(2));
                resultsUrlRep.Add(reader.GetString(3));
            }
        }
    }
    catch (Exception ex)
    {
        System.Windows.Forms.MessageBox.Show(ex.ToString());
        System.Diagnostics.Debug.WriteLine(ex.ToString());
    }

    if (resultsQuestions.Count == 0)
    {
        MessageBox.Show("Ce quizz ne possède aucune question");
        if (Application.Current.Properties["nameindex"]!.ToString()!.Contains("Marked"))
        {
            NavigationService.Instance.Navigate("MainContent", new VMarked(mainVm));
        }
        else
        {
            NavigationService.Instance.Navigate("MainContent", new VQuizz(mainVm));
        }
    }
    return (resultsQuestions, resultsReponses, resultsUrlQuestion, resultsUrlRep); 
}
        public static (List<string>, List<string>, List<string>, List<string>) RetrievequizzToCreated(string? nameindex)
        {
            var connection = new SQLiteConnection(ConSource);
            var query = "SELECT question,reponse,image_question_url,image_answer_url  FROM " +
                        App.Current.Properties["matier"].ToString() + " WHERE name = \"" + nameindex +
                        "\" AND ID = \"100\"";
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    var reader = command.ExecuteReader();
                    Aquestion.Clear();
                    Arepnse.Clear();
                    AurlQuestion.Clear();
                    AurlRep.Clear();
                    while (reader.Read())
                    {
                        Aquestion.Add(reader.GetString(0));
                        Arepnse.Add(reader.GetString(1));
                        AurlQuestion.Add(reader.GetString(2));
                        AurlRep.Add(reader.GetString(3));
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                return (Aquestion, Arepnse, AurlQuestion, AurlRep);
                connection.Close();
        }
        
        public static (List<string>, List<string>, List<string>, List<string>) GetMarked(string mat)
        {
            List<string> question = new List<string>();
            List<string> rep = new List<string>();
            List<string> urlRep = new List<string>();
            List<string> urlQuestion = new List<string>();
            var connection = new SQLiteConnection(ConSource);
            string valtoreq = "question,reponse,image_question_url,image_answer_url";
            string query = "";
            if (mat == "All")
            {
                query = "SELECT " + valtoreq + " FROM Marked ";
            }
            else
            {
                query = "SELECT " + valtoreq + " FROM Marked where Matier = \"" + mat + "\"";
            }

            using (var db = new SQLiteConnection(ConSource))
            {
                db.Open();
                using (var cmd = new SQLiteCommand(query, db))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        question.Add(reader.GetString(0));
                        rep.Add(reader.GetString(1));
                        urlQuestion.Add(reader.GetString(2));
                        urlRep.Add(reader.GetString(3));

                    }
                }
            }

            return (question, rep, urlQuestion, urlRep);
        }
        public static List<string> GetMarkedForQUizz(string mat)
        {
            List<string> question = new List<string>();
            string valtoreq = "question";
            string query = "";
            if (mat == "All")
            {
                query = "SELECT " + valtoreq + " FROM Marked where Matier = \"" +
                        App.Current.Properties["nameindex"].ToString() + "\"";
            }
            else
            {
                query = "SELECT " + valtoreq + " FROM Marked where Matier = \"" + mat + "\"";
            }

            using (var db = new SQLiteConnection(ConSource))
            {
                db.Open();
                using (var cmd = new SQLiteCommand(query, db))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        question.Add(reader.GetString(0));
                    }
                }
            }

            return question;
        }


        public static List<string> GetAllName()
        {
            Name.Clear();
            var query = "Select name FROM " + ListMat[0];
            for (int i = 1; i < ListMat.Count; i++)
            {
                query += " UNION Select name FROM " + ListMat[i];
            }

            using (var db = new SQLiteConnection(ConSource))
            {
                db.Open();
                using (var cmd = new SQLiteCommand(query, db))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var value = reader.GetString(0);
                        if (!Name.Contains(value))
                        {
                            Name.Add(value);
                        }
                    }
                }
            }

            return Name;
        }

        public static List<string> GetNameCreated(string? nom)
        {
            Name.Clear();
            var query = "SELECT name FROM " + nom + " WHERE ID = \"100\"";
            using var db = new SQLiteConnection(ConSource);
            db.Open();
            using var cmd = new SQLiteCommand(query, db);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var value = reader.GetString(0);
                if (!Name.Contains(value))
                {
                    Name.Add(value);
                }
            }

            return Name;
        }



        public static void SaveQuizz(string matier, string name, string question, string imgQuestion, string rep,
            string imgRep)
        {
            string query = "";
            matier = "\"" + matier + "\"";
            name = "\"" + name + "\"";
            question = "\"" + question + "\"";
            imgQuestion = "\"" + imgQuestion + "\"";
            rep = "\"" + rep + "\"";
            imgRep = "\"" + imgRep + "\"";
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(ConSource))
                {
                    c.Open();
                    query = "SELECT COUNT(*) FROM " + matier + " WHERE  name = " + name + " AND question = " + question;
                    System.Diagnostics.Debug.WriteLine(query);
                    using (SQLiteCommand cmd = new SQLiteCommand(query, c))
                    {
                        long count = (long)cmd.ExecuteScalar();
                        System.Diagnostics.Debug.WriteLine(count);

                        if (count == 0)
                        {
                            query = "INSERT INTO " + matier +
                                    " (id,difficulty ,name,question,reponse,image_question_url,image_answer_url,Marked) VALUES (100,1," +
                                    name + "," + question + "," + rep + "," + imgQuestion + "," + imgRep + ",0)";
                            using (SQLiteCommand insertCmd = new SQLiteCommand(query, c))
                            {
                                Console.WriteLine(query);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(query);
                Console.WriteLine(ex.Message);
                MessageBox.Show("An error occured while saving your quizz");
            }
        }

        public static void DeleteCreated(string question)
        {
            using (SQLiteConnection c = new SQLiteConnection(ConSource))
            {
                c.Open();
                string query = "DELETE FROM " + App.Current.Properties["matier"].ToString() +
                               " WHERE REPLACE(question, ' ', '') =  REPLACE(\"" + question +
                               "\", ' ', '') AND ID = \"100\"";
                Console.WriteLine(query);
                using (SQLiteCommand cmd = new SQLiteCommand(query, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static (string? matier, string? name, string question, string? reponse, string? imageQuestion, string?
            imageRep) SearchQuizzCreated(string question)
        {
            string? matier = null;
            string? name = null;
            string? reponse = null;
            string? imageQuestion = null;
            string? imageRep = null;
            var connection = new SQLiteConnection(ConSource);
            string query = "";
            query = "SELECT name,reponse,image_question_url,image_answer_url FROM " +
                    App.Current.Properties["matier"].ToString() +
                    " WHERE ID = \"100\" AND REPLACE(question, ' ', '') =  REPLACE(\"" + question +
                    "\", ' ', '') AND name = \"" + App.Current.Properties["nameindex"].ToString() + "\"";
            using (var db = new SQLiteConnection(ConSource))
            {
                db.Open();
                using (var cmd = new SQLiteCommand(query, db))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        matier = App.Current.Properties["matier"].ToString();
                        name = reader.GetString(0);
                        question = question;
                        reponse = reader.GetString(1);
                        imageQuestion = reader.GetString(2);
                        imageRep = reader.GetString(3);
                    }
                }
            }

            return (matier, name, question, reponse, imageQuestion, imageRep);
        }
        
    public static void ReplaceQuizz(string matier, string name, string question, string imageQuestion, string reponse,string imageRep)
        {
            string query = "";
            name = "\"" + name + "\"";
            question = "\"" + question + "\"";
            imageQuestion = "\"" + imageQuestion + "\"";
            reponse = "\"" + reponse + "\"";
            imageRep = "\"" + imageRep + "\"";
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(ConSource))
                {
                    c.Open();
                    query = "UPDATE " + matier + " SET name = " + name +  ", question = " + question + ",reponse = " + reponse  + ", image_question_url = " + imageQuestion + ", image_answer_url = " + imageRep +" WHERE ID = \"100\" AND name = \"" + App.Current.Properties["nameindex"].ToString() + "\" AND  REPLACE(question, ' ', '') =  REPLACE(\"" + App.Current.Properties["old_quest"].ToString() + "\", ' ', '')";
                    Console.WriteLine(query);
                    using (SQLiteCommand cmd = new SQLiteCommand(query, c))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(query);
                Console.WriteLine(ex.Message);
                MessageBox.Show("An error occured while saving your quizz");
            }
        }
        public static (List<string> level, List<string> course, List<string> question, List<string> imageQuestion,
            List<string> reponse, List<string> imageRep, List<string> difficulty) GetAllPublic()
        {
            var nameTable = GetTable("PublicDB");
            string query = "";
            using (SQLiteConnection c = new SQLiteConnection(Tempsource))
            {
                c.Open();
                foreach (var i in nameTable)
                {
                    query = "SELECT * FROM " +  i.ToString();
                    using (var cmd = new SQLiteCommand(query , c))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Level.Add(reader.GetString(0)) ;
                            Course.Add(reader.GetString(1)) ;
                            Question.Add(reader.GetString(2));
                            Reponse.Add(reader.GetString(3)) ;
                            ImageQuestion.Add(reader.GetString(4)) ;
                            ImageRep.Add(reader.GetString(5)) ;
                            Difficulty.Add(reader.GetString(6)) ;
                        }
                    }
                }
            }
            return (Level, Course, Question, ImageQuestion, Reponse, ImageRep, Difficulty);
        }

        public static List<string> GetTable(string DB)
        {
            switch (DB)
            {
                case "PublicDB":
                    string query = "";
                    using (var db = new SQLiteConnection(Tempsource))
                    {
                        db.Open();
                        query = "SELECT name from sqlite_master";
                        using (var cmd = new SQLiteCommand(query, db))
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TempList.Add(reader.GetString(0));
                            }
                        }
                    }

                    break;
            }
            return TempList;
        }
        
        
        
    }
}
