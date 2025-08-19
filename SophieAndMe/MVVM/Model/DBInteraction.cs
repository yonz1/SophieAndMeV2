using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text;
using System.Text.Json;
using System.Windows;
using SophieAndMe.Core;
using SophieAndMe.MVVM.View;
using SophieAndMe.MVVM.ViewModel;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;


namespace SophieAndMe.MVVM.Model
{
    abstract class DbInteraction
    {

        static DbInteraction()
        {
            _dataService = App.DataService;
        }
        // public DbInteraction()
        // {
        //     _dataService = App.DataService;
        // }
        
        // ################################################### Initialisations 

        private static readonly List<string> Aquestion = new List<string>();
        private static readonly List<string> Arepnse = new List<string>();
        private static readonly List<string> AurlQuestion = new List<string>();
        private static readonly List<string> AurlRep = new List<string>();
        private static readonly List<string> Name = new List<string>();

        private static readonly List<string> ListMat =
            ["Mathématiques", "Physique", "SI", "Français", "Anglais", "Erreurs"];


        private static readonly string UserSource = "Data Source=..//..//..//Database//user_value.db";
        private static readonly string ConSource = "Data Source=..//..//..//Database//data_restored.db";
        private static readonly string Tempsource = "Data Source=..//..//..//Database//PublicDB.db";
        private static readonly string ProgressSource = "Data Source=..//..//..//Database//data_progressif.db";
        private static readonly string EDTSource = "Data Source=..//..//..//Database//EDT.db";


        private static readonly List<string> Level = new List<string>();
        private static readonly List<string> Course = new List<string>();
        private static List<string> Question = new List<string>();
        private static List<string> ImageQuestion = new List<string>();
        private static List<string> Reponse = new List<string>();
        private static List<string> ImageRep = new List<string>();
        private static readonly List<string> Difficulty = new List<string>();

        private static readonly List<string> TempList = [];

        private static IDataService _dataService;
        
        // ################################################### Fonctions


        public static void MarkData(string question, string repnse, string urlQuestion, string urlRep)
        {
            using SQLiteConnection c = new SQLiteConnection(ConSource);
            c.Open();
            var query = "SELECT COUNT(*) FROM Marked WHERE  question = \"" + question + "\" AND Matier =   \"" +
                        _dataService.QuizzId.Matier?.ToString() + "\"";
            using SQLiteCommand cmd = new SQLiteCommand(query, c);
            long count = (long)cmd.ExecuteScalar();

            if (count == 0)
            {
                var mat = "\"" + _dataService.QuizzId.Matier.ToString() + "\",";
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
            string query = "";

            try
            {
                connection.Open();
                SQLiteCommand command;
                if (_dataService.QuizzId.IsAll)
                {
                    var chapter = _dataService.SharedListChapter;
                    var sb = new StringBuilder();
                    var parameters = new List<SQLiteParameter>();
                    Console.WriteLine("All");
                    for (int i = 0; i < chapter.Count; i++)
                    {
                        Console.WriteLine(chapter[i]);
                        if (i > 0) sb.Append(" UNION ");
                        sb.Append(
                            $"SELECT question, reponse, image_question_url, image_answer_url,name FROM {nameindex} WHERE REPLACE(name, ' ', '') = REPLACE(@name{i}, ' ', '')");
                        parameters.Add(new SQLiteParameter($"@name{i}", chapter[i]));
                    }

                    command = new SQLiteCommand(sb.ToString(), connection);
                    command.Parameters.AddRange(parameters.ToArray());
                    _dataService.QuizzId.Matier = nameindex;
                }
                else if (nameindex != null && nameindex.Contains("Marked"))
                {
                    Console.WriteLine("Marked");
                    query = "SELECT question,reponse,image_question_url,image_answer_url FROM Marked WHERE Matier = @matier";
                    command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@matier", _dataService.QuizzId.Matier?.ToString());
                }

                else
                {
                    Console.WriteLine("normale");
                    query =
                        $"SELECT question,reponse,image_question_url,image_answer_url FROM {_dataService.QuizzId.Matier} WHERE name = @name";
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
                Console.WriteLine(ex.ToString());
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
                        _dataService.QuizzId.Matier.ToString() + " WHERE name = \"" + nameindex +
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
            if (_dataService.QuizzId.IsAll)
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
            if (_dataService.QuizzId.IsAll)
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
                string query = "DELETE FROM " + _dataService.QuizzId.Matier.ToString() +
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
                    _dataService.QuizzId.Matier.ToString() +
                    " WHERE ID = \"100\" AND REPLACE(question, ' ', '') =  REPLACE(\"" + question +
                    "\", ' ', '') AND name = \"" + App.Current.Properties["nameindex"].ToString() + "\"";
            Console.WriteLine(query);
            using (var db = new SQLiteConnection(ConSource))
            {
                db.Open();
                using (var cmd = new SQLiteCommand(query, db))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        matier = _dataService.QuizzId.Matier.ToString();
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

        public static void ReplaceQuizz(string matier, string name, string question, string imageQuestion,
            string reponse, string imageRep)
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
                    query = "UPDATE " + matier + " SET name = " + name + ", question = " + question + ",reponse = " +
                            reponse + ", image_question_url = " + imageQuestion + ", image_answer_url = " + imageRep +
                            " WHERE ID = \"100\" AND name = \"" + App.Current.Properties["nameindex"].ToString() +
                            "\" AND  REPLACE(question, ' ', '') =  REPLACE(\"" +
                            App.Current.Properties["old_quest"].ToString() + "\", ' ', '')";
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
                    query = "SELECT * FROM " + i.ToString();
                    using (var cmd = new SQLiteCommand(query, c))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Level.Add(reader.GetString(0));
                            Course.Add(reader.GetString(1));
                            Question.Add(reader.GetString(2));
                            Reponse.Add(reader.GetString(3));
                            ImageQuestion.Add(reader.GetString(4));
                            ImageRep.Add(reader.GetString(5));
                            Difficulty.Add(reader.GetString(6));
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


        // ########################################################## Collection de fonction pour les Quizz progressifs

        public static bool VerifStart()
        {
            string query = "";
            var Mat = _dataService.QuizzId.Matier;
            using (var db = new SQLiteConnection(ProgressSource))
            {
                db.Open();
                query = "SELECT COUNT(*) from Progressions"+ Mat;
                using (var cmd = new SQLiteCommand(query, db))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32(0) == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        public static void RegisterProgression(MainViewModel mainVm)
        {
            DeleteProg();
            using SQLiteConnection c = new SQLiteConnection(ProgressSource);
            c.Open();
            string query = "";
            _dataService = App.DataService;
            var chapter = _dataService.SharedListChapter;
            var Mat = _dataService.QuizzId.Matier;
            SQLiteCommand command;
            foreach (var info in chapter)
            {
                Console.WriteLine("data -" + info);
                query = "INSERT INTO Chapitre" + Mat + " (Noms) VALUES (@val)";
                command = new SQLiteCommand(query, c);
                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@val";
                param.Value = info;
                command.Parameters.Add(param);
                command.ExecuteNonQuery();
            }
            
            (Question, Reponse, ImageQuestion, ImageRep) = Retrievequizz(Application.Current.Properties["nameindex"]?.ToString(), "", mainVm);
            for (int i = 0; i < Question.Count; i++)
            {
                query =
                    "INSERT INTO Progressions" + Mat +" (question,reponse,image_question_url,image_answer_url) VALUES (@question,@reponse,@imageQuestion,@imageRep)";
                command = new SQLiteCommand(query, c);
                string val = "Progressions" + Mat;
                command.Parameters.AddWithValue("@Prog", val);
                command.Parameters.AddWithValue("@question", Question[i]);
                command.Parameters.AddWithValue("@reponse", Reponse[i]);
                command.Parameters.AddWithValue("@imageQuestion", ImageQuestion[i]);
                command.Parameters.AddWithValue("@imageRep", ImageRep[i]);
                command.ExecuteNonQuery();
            }
        }

        public static (List<string>, List<string>, List<string>, List<string>) RetrievequizzToProg()
        {
            var connection = new SQLiteConnection(ProgressSource);
            var resultsQuestions = new List<string>();
            var resultsReponses = new List<string>();
            var resultsUrlQuestion = new List<string>();
            var resultsUrlRep = new List<string>();
            var resultsName = new List<string>();
            connection.Open();
            SQLiteCommand command;
            string query = "SELECT question,reponse,image_question_url,image_answer_url FROM Progressions" + _dataService.QuizzId.Matier;
            command = new SQLiteCommand(query, connection);
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
            return (resultsQuestions,resultsReponses,resultsUrlQuestion,resultsUrlRep);
        }

        public static void DeleteProg()
        {
            using SQLiteConnection c = new SQLiteConnection(ProgressSource);
            c.Open();
            SQLiteCommand command;
            string query = "DELETE FROM Progressions" +  _dataService.QuizzId.Matier;
            command = new SQLiteCommand(query, c);
            command.ExecuteNonQuery();
            query = "DELETE FROM Chapitre" +  _dataService.QuizzId.Matier;
            command = new SQLiteCommand(query, c);
            command.ExecuteNonQuery();
        }

        public static void DeleteQuestion(List<string> Question)
        {
            using SQLiteConnection c = new SQLiteConnection(ProgressSource);
            c.Open();
            SQLiteCommand command;
            foreach (var data in Question)
            {
                string query = "DELETE FROM Progressions" +  _dataService.QuizzId.Matier + " WHERE REPLACE(question,'\"', '''') =  @data"; 
                Console.WriteLine(data);
                command = new SQLiteCommand(query, c);
                command.Parameters.AddWithValue("@data", data);
                command.ExecuteNonQuery();                
            }
        }


        public static string GetMat(string Name)
        {
            Dictionary<string, string> mainDic= new Dictionary<string, string>();
            foreach (var info in ListMat)
            {
                foreach (var Chapter in GetName(info))
                {
                    mainDic[Chapter] = info;
                }
            }
            return mainDic[Name];
        }

        
        
        // ########################################################## Collection de fonction pour le Landing
        public static (List<string>, List<string>) GetNotesWeb()
        {
            List<string> Anglais =  new List<string>();
            List<string> Français = new List<string>();
            List<string> Maths = new List<string>();
            List<string> Physique =  new List<string>();
            List<string> SI = new List<string>();
            Dictionary<string?, string> Main = new Dictionary<string?, string>();
            using SQLiteConnection c = new SQLiteConnection(UserSource);
            c.Open();
            SQLiteCommand command;
            string query = "SELECT Anglais,Français,Maths,Physique,SI FROM Plus ";
            command = new SQLiteCommand(query, c);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Anglais.Add(reader.GetString(0));
                    Français.Add(reader.GetString(1));
                    Maths.Add(reader.GetString(2));
                    Physique.Add(reader.GetString(3));
                    SI.Add(reader.GetString(4));
                }
            }
            var matiere = new List<List<string>> { Anglais, Français, Maths, Physique, SI };
            var matiereStr = new List<string> { "Anglais", "Français", "Maths", "Physique", "SI" };
            for (int i = 0; i < matiere.Count ; i++)
            {
                if (matiere[i][Anglais.Count-1].ToString() != "Pas de colle")
                {
                    string[] val = matiere[i][Anglais.Count-1].ToString().Split(";");
                    Main[matiereStr[i]] = $"{val[0]} - {val[4]} - {val[2].Replace("Moy:","")}";
                }
            }
            return(Main.Keys.ToList(),Main.Values.ToList());
        }


        public static List<string> GetQuizzWeb()
        {
            List<string> Name = [];
            using SQLiteConnection c = new SQLiteConnection(UserSource);
            c.Open();
            SQLiteCommand command;
            string query = "SELECT Name FROM DATE ORDER BY Inserted DESC LIMIT 3";
            command = new SQLiteCommand(query, c);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetString(0));
                    Name.Add(reader.GetString(0));
                }
            }
            return Name;
        }
        
        // ########################################################## Collection de fonction pour l'emploi du temps
        
        
        public static (List<string>,List<string>,List<string>) GetPlanning(string Days, string Semaine)
        {
            List<string> Mat =  new List<string>();
            List<string> Salle = new List<string>();
            List<string> Ensei = new List<string>();
            using SQLiteConnection c = new SQLiteConnection(EDTSource);
            c.Open();
            SQLiteCommand command;
            string query = "SELECT Mat,Salle,Enseignant FROM " + Days + Semaine;
            command = new SQLiteCommand(query, c);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Mat.Add(reader.GetString(0));
                    Salle.Add(reader.GetString(1));
                    Ensei.Add(reader.GetString(2));
                }
            }
            return (Mat,Salle,Ensei);
        }
        
    }
}
