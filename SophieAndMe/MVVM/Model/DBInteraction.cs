
using System.Data.SQLite;
using System.Windows.Forms;
using SophieAndMe.Core;
using SophieAndMe.MVVM.View;
using SophieAndMe.MVVM.ViewModel;


namespace SophieAndMe.MVVM.Model
{
    class DBInteraction
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

        
        public static List<string> level = new List<string>();
        public static List<string> course = new List<string>();
        public static List<string> Question = new List<string>();
        public static List<string> imageQuestion = new List<string>();
        public static List<string> reponse = new List<string>();
        public static List<string> imageRep = new List<string>();
        public static List<string> difficulty = new List<string>();

        public static List<string> TempList = [];
        // ################################################### Fonctions


        public static void MarkData(string question, string repnse, string urlQuestion, string urlRep)
        {
            string query = "";
            using (SQLiteConnection c = new SQLiteConnection(ConSource))
            {
                c.Open();
                query = "SELECT COUNT(*) FROM Marked WHERE  question = \"" + question + "\" AND Matier =   \"" +
                        App.Current.Properties["matier"].ToString() + "\"";
                using (SQLiteCommand cmd = new SQLiteCommand(query, c))
                {
                    long count = (long)cmd.ExecuteScalar();

                    if (count == 0)
                    {
                        string mat = "\"" + App.Current.Properties["matier"].ToString() + "\",";
                        string quest = "\"" + question.Replace("\\/", "/") + "\",";
                        string rep = "\"" + repnse + "\",";
                        string questionImg = "\"" + urlQuestion + "\",";
                        string reponseImg = "\"" + urlRep + "\"";
                        query =
                            "INSERT INTO Marked (Matier,question,reponse,image_question_url,image_answer_url) VALUES (" +
                            mat + quest + rep + questionImg + reponseImg + ")";
                        using (SQLiteCommand insertCmd = new SQLiteCommand(query, c))
                        {
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
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

        public static List<string> GetName(string? Mat)
        {
            var result = new List<string>();
            if (Mat != "All")
            {
                using (var db = new SQLiteConnection(ConSource))
                {
                    db.Open();
                    string query = $"SELECT name FROM [{Mat}] ORDER BY name";
                    using (var cmd = new SQLiteCommand(query, db))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nom = reader.GetString(0);
                            if (!result.Contains(nom))
                                result.Add(nom);
                        }
                    }
                }

                using (var db = new SQLiteConnection(ConSource))
                {
                    db.Open();
                    string query = $"SELECT name FROM [{Mat}] ORDER BY name";
                    using (var cmd = new SQLiteCommand(query, db))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nom = reader.GetString(0);
                            if (!result.Contains(nom))
                                result.Add(nom);
                        }
                    }
                }
            }
            else
            {
                result = ListMat;
            }

            return result;
        }

        public static (List<string>, List<string>, List<string>, List<string>) Retrievequizz(string? nameindex,
            string ID, MainViewModel mainVm)
        {
            var connection = new SQLiteConnection(ConSource);
            string query = "";
            if ((string)App.Current.Properties["matier"] == "All")
            {
                query = "SELECT question,reponse,image_question_url,image_answer_url FROM " + nameindex;
                App.Current.Properties["matier"] = nameindex;
            }
            else if (nameindex != null && nameindex.Contains("Marked"))
            {
                query = "SELECT question,reponse,image_question_url,image_answer_url FROM Marked Where Matier = \"" +
                        App.Current.Properties["matier"].ToString() + "\"";
            }
            else
            {
                query = "SELECT question,reponse,image_question_url,image_answer_url  FROM " +
                        App.Current.Properties["matier"].ToString() + " WHERE name = \"" + nameindex + "\"";
            }

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

            if (Aquestion.Count == 0)
            {
                MessageBox.Show("Ce quizz ne posséde aucune questions");
                if (App.Current.Properties["nameindex"].ToString().Contains("Marked"))
                {
                    NavigationService.Instance.Navigate(new VMarked(mainVm));
                }
                else
                {
                    NavigationService.Instance.Navigate(new VQuizz(mainVm));
                }
            }

            return (Aquestion, Arepnse, AurlQuestion, AurlRep);
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
                            level.Add(reader.GetString(0)) ;
                            course.Add(reader.GetString(1)) ;
                            Question.Add(reader.GetString(2));
                            reponse.Add(reader.GetString(3)) ;
                            imageQuestion.Add(reader.GetString(4)) ;
                            imageRep.Add(reader.GetString(5)) ;
                            difficulty.Add(reader.GetString(6)) ;
                        }
                    }
                }
            }
            return (level, course, Question, imageQuestion, reponse, imageRep, difficulty);
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
