using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text;
using System.Text.Json;
using System.Windows;
using Microsoft.VisualBasic.ApplicationServices;
using System.Globalization;
using System.Windows.Media;
using SophieAndMe.Core;
using SophieAndMe.MVVM.View;
using SophieAndMe.MVVM.ViewModel;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;
using SophieAndMe.MVVM.Model;


namespace SophieAndMe.MVVM.Model
{
    abstract class DbInteraction
    {

        static DbInteraction()
        {
            _dataService = App.DataService;
        }
        
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
        
        public static System.Windows.Media.Color MatToColorQuizz(string Mat)
        {
            System.Windows.Media.Color val;
            switch (Mat)
            {
                case "Mathématiques":
                    val = System.Windows.Media.Color.FromRgb(30, 110, 244);
                    break;
                case "Physique":
                    val = System.Windows.Media.Color.FromRgb(52, 199, 89);
                    break;
                case"SI":
                    val = System.Windows.Media.Color.FromRgb(255, 141, 40);
                    break;
                case "Anglais":
                    val = System.Windows.Media.Color.FromRgb(179, 136, 255);
                    break;
                case "Français":
                    val = System.Windows.Media.Color.FromRgb(59, 221, 236);
                    break;
                case "Erreurs":
                    val = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
                    break;
            }
            return val;
        }

        public static SortedDictionary<string,SolidColorBrush> GetName(object? mat)
        {
            var result = new List<string>();
            SortedDictionary<string,SolidColorBrush> valueret = new SortedDictionary<string, SolidColorBrush>();
            if ((string)mat! == "Tous")
            {
                using var db = new SQLiteConnection(ConSource);
                db.Open();
                foreach (var Matier in  ListMat)
                {
                    SolidColorBrush color = new SolidColorBrush(MatToColorQuizz(Matier));
                    string query = $"SELECT DISTINCT r.name FROM {Matier} AS D JOIN Reference AS r ON d.name = r.id ORDER BY r.name";
                    using var cmd = new SQLiteCommand(query, db);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string nom = reader.GetString(0);
                        if (!result.Contains(nom))
                        {
                            result.Add(nom);
                            valueret[nom] = color;
                        }
                    }
                    
                }
            }
            else if ((string)mat! != "All")
            {
                SolidColorBrush color =  new SolidColorBrush(MatToColorQuizz((string)mat!));
                using var db = new SQLiteConnection(ConSource);
                db.Open();
                string query = $"SELECT DISTINCT r.name FROM {mat} AS D JOIN Reference AS r ON d.name = r.id ORDER BY r.name";
                Console.WriteLine(query);
                using var cmd = new SQLiteCommand(query, db);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string nom = reader.GetString(0);
                    result.Add(nom);
                    valueret[nom] = color;
                }
            }
            else
            {
                result = ListMat;
                foreach (var matVal in result)
                {
                    Color color = MatToColorQuizz(matVal);
                    valueret[matVal] = new SolidColorBrush(color);
                }
            }
            // valueret.Sort();

            return valueret;
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
                    // query =
                    //     $"SELECT question,reponse,image_question_url,image_answer_url FROM {_dataService.QuizzId.Matier} WHERE name = @name";
                    query =
                        $"SELECT m.question,m.reponse,m.image_question_url,m.image_answer_url FROM {_dataService.QuizzId.Matier} AS m JOIN Reference AS r ON r.id = m.name WHERE r.name = @name";
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
                if (_dataService.QuizzId.Nameindex!.ToString()!.Contains("Marked"))
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
            var query = $"SELECT question,reponse,image_question_url,image_answer_url FROM {_dataService.QuizzId.Matier.ToString()} AS m JOIN Reference AS r ON m.name = r.id WHERE r.Name = \"{nameindex}\"";
            Console.WriteLine(query);
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

            Console.WriteLine("A");
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
                        _dataService.QuizzId.Nameindex.ToString() + "\"";
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
            var query = "SELECT Name FROM Reference";

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
            Name.Clear();
            var query = "SELECT Name FROM Reference AS r JOIN Reference_mat AS m ON r.IdMatier = m.id WHERE m.Matier = \"" + nom +  "\" AND r.Created = \"1\"";
            if (nom == "All")
            {
                query = "SELECT Name From Reference WHERE Created = \"1\"";
            }
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


        public static void VerifReference(string name,string matier)
        {
            using (SQLiteConnection c = new SQLiteConnection(ConSource))
            {
                    
                c.Open();
                string query = $"SELECT Count(name) FROM Reference WHERE name = \"{name}\"";
                System.Diagnostics.Debug.WriteLine(query);
                using (SQLiteCommand cmd = new SQLiteCommand(query, c))
                {
                    long count = (long)cmd.ExecuteScalar();
                    System.Diagnostics.Debug.WriteLine(count);

                    if (count == 0)
                    {
                        query = $"INSERT INTO Reference (name,Created,IdMatier) VALUES (\"{name}\",1,(SELECT id FROM Reference_mat WHERE Matier = \"{matier}\"))";
                        using (SQLiteCommand insertCmd = new SQLiteCommand(query, c))
                        {
                            Console.WriteLine(query);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static void SaveQuizz(string matier, string name, string question, string imgQuestion, string rep,
            string imgRep)
        {
            string query = "";
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(ConSource))
                {
                    VerifReference(name,matier );
                    c.Open();
                    query = $"SELECT COUNT(*) FROM {matier} AS m JOIN Reference AS r ON r.id = m.name WHERE r.Name = \"{name}\" AND replace(m.question,' ','') = replace(\"{question}\",' ','')  ";
                    System.Diagnostics.Debug.WriteLine(query);
                    using (SQLiteCommand cmd = new SQLiteCommand(query, c))
                    {
                        long count = (long)cmd.ExecuteScalar();
                        System.Diagnostics.Debug.WriteLine(count);

                        if (count == 0)
                        {
                            query = $"INSERT INTO {matier} (id,difficulty ,name,question,reponse,image_question_url,image_answer_url,Marked) VALUES (100,1,(SELECT id FROM Reference WHERE Name = \"{name}\"),\"{question}\",\"{rep}\",\"{imgQuestion}\",\"{imgRep}\",0)";
                            using (SQLiteCommand insertCmd = new SQLiteCommand(query, c))
                            {
                                Console.WriteLine(query);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                using (SQLiteConnection c = new SQLiteConnection(UserSource))
                {
                    c.Open();
                    query = $"SELECT COUNT(*) FROM Date WHERE  name = \"{name}\"";
                    System.Diagnostics.Debug.WriteLine(query);
                    using (SQLiteCommand cmd = new SQLiteCommand(query, c))
                    {
                        long count = (long)cmd.ExecuteScalar();
                        System.Diagnostics.Debug.WriteLine(count);
                
                        if (count == 0)
                        {
                            query = $"INSERT INTO Date (Name,Inserted) VALUES (\"{name}\",\"{DateTime.UtcNow.Date.ToString("yyyy-MM-dd HH:mm:ss")}\")";
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

        public static void DeleteCreated(string question,string name)
        {
            List<string> DeleteList = new List<string>();
            using (SQLiteConnection c = new SQLiteConnection(ConSource))
            {
                c.Open();
                string query = $"DELETE FROM {_dataService.QuizzId.Matier.ToString()} AS m WHERE replace(m.question, ' ','') = replace(\"{question}\",' ','')";
                Console.WriteLine(query);
                using (SQLiteCommand cmd = new SQLiteCommand(query, c))
                {
                    cmd.ExecuteNonQuery();
                }

                query = $"SELECT r.Name FROM Reference AS r JOIN Reference_mat AS rm ON rm.id = r.IdMatier WHERE Matier = \"{_dataService.QuizzId.Matier.ToString()}\" EXCEPT SELECT r.Name FROM {_dataService.QuizzId.Matier.ToString()} AS m JOIN Reference AS r ON r.id = m.name GROUP BY r.id";
                Console.WriteLine(query);
                using (var cmd = new SQLiteCommand(query,c))
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine(reader.GetString(0));
                    DeleteList.Add(reader.GetString(0));
                }

                foreach (var Name in  DeleteList)
                {
                    query = $"DELETE FROM Reference WHERE Name = \"{Name}\"";
                    Console.WriteLine(query);
                    using (SQLiteCommand cmd = new SQLiteCommand(query, c))
                    {
                        cmd.ExecuteNonQuery();
                    }
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
            query = $"SELECT r.name,m.reponse,m.image_question_url,m.image_answer_url FROM {GetMat(_dataService.QuizzId.Nameindex.ToString())} AS m JOIN Reference AS r ON r.id = m.name WHERE REPLACE(question, ' ', '') = REPLACE(\"" + question + "\", ' ','') AND REPLACE(r.Name, ' ','') = REPLACE(\"" + _dataService.QuizzId.Nameindex.ToString() +"\",' ','')";
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
            Console.WriteLine("######################################################");
            Console.WriteLine("Modification pour éditions ");
            Console.WriteLine("Matier : " + matier);
            Console.WriteLine("Name : " + name);
            Console.WriteLine("Question : " + question);
            Console.WriteLine("Rep : " + reponse);
            Console.WriteLine("Image Question : " + imageQuestion);
            Console.WriteLine("Image Rep : " + imageRep);
            return (matier, name, question, reponse, imageQuestion, imageRep);
        }

        public static void ReplaceQuizz(string matier, string name, string question, string imageQuestion,
            string reponse, string imageRep)
        {
            string query = "";

            try
            {
                using (SQLiteConnection c = new SQLiteConnection(ConSource))
                {
                    c.Open();
                    query = $"UPDATE {matier} SET name = (SELECT id FROM Reference WHERE Name = \"{name}\"), question = \"{question}\", reponse = \"{reponse}\", image_question_url = \"{imageQuestion}\",image_answer_url = \"{imageRep}\" WHERE name = (SELECT id FROM Reference WHERE Name = \"{_dataService.QuizzId.Nameindex.ToString()}\") AND replace(question,' ','') = replace(\"{_dataService.QuizzId.old_quest.ToString()}\",' ','')";
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
                query = "INSERT INTO Chapitre" + Mat + " (Noms) VALUES (@val)";
                command = new SQLiteCommand(query, c);
                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@val";
                param.Value = info;
                command.Parameters.Add(param);
                command.ExecuteNonQuery();
            }
            
            (Question, Reponse, ImageQuestion, ImageRep) = Retrievequizz(_dataService.QuizzId.Nameindex?.ToString(), "", mainVm);
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
                command = new SQLiteCommand(query, c);
                command.Parameters.AddWithValue("@data", data);
                command.ExecuteNonQuery();                
            }
        }


        public static string GetMat(string Name)
        {
            using SQLiteConnection c = new SQLiteConnection(ConSource);
            c.Open();
            string Matier = "";
            SQLiteCommand command;
            string query = $"SELECT rm.Matier FROM Reference as r JOIN Reference_mat AS rm ON rm.id = r.IdMatier WHERE r.Name = \"{Name}\"";
            command = new SQLiteCommand(query,c);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Matier = (reader.GetString(0));
                }
            } 
            Console.WriteLine("Matier: " + Matier);
            return Matier;
        }

        
        
        // ########################################################## Collection de fonction pour le Landing
        
        public static Dictionary<string, string> RetrieveAchieved()
        {
            List<string> matier = ["Mathématiques", "Physique", "SI", "Français", "Anglais","Erreurs"];
            Dictionary<string, string> dico = new Dictionary<string, string>();
            using SQLiteConnection c = new SQLiteConnection(ConSource);
            c.Open();
            SQLiteCommand command;
            for (int i = 0; i < 6; i++)
            {
                string query = $"SELECT  sum(Ended),count(*) FROM {matier[i]} ";
                Console.WriteLine(query);
                command = new SQLiteCommand(query, c);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ended = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        int total = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                        Console.WriteLine(ended);
                        Console.WriteLine(total);

                        
                        float ratio = total > 0 ? (float)ended / total : 0f;
                        ratio = ended == 0 ? 0 : ratio;
                        Console.WriteLine(matier[i] + " : " + ratio);
                        dico[matier[i]] = (ratio * 100).ToString(); // en pourcentage formaté
                    }
                }
            }
            return dico;
        }
        
        public static Dictionary<string, string> RetrieveTimePassed(List<DateTime> date)
        {
            List<string> Days = ["lundi", "mardi", "mercredi", "jeudi", "vendredi", "samedi", "dimanche"];
            Dictionary<string, string> dico = new Dictionary<string, string>();
            using SQLiteConnection c = new SQLiteConnection(UserSource);
            c.Open();
            SQLiteCommand command;
            for (int i = 0; i < 7; i++)
            {
                string query = $"SELECT TimePassed FROM Time WHERE DATE = \"{date[i].ToString("yyyy-MM-dd")}\"";
                Console.WriteLine(query);
                command = new SQLiteCommand(query, c);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string val = reader.IsDBNull(0) ? "0" : reader.GetString(0);
                        Console.WriteLine(val);
                        dico[Days[i]] = val;
                    }
                }
            }
            return dico;
        }
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
                    Main[matiereStr[i]] = $"{val[0]};{val[4]}";
                }
            }
            Console.WriteLine(Main.Count);
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
                    Name.Add(reader.GetString(0));
                }
            }

            // foreach (var name in Name)
            // {
            //     Console.WriteLine(name);
            // } 
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

        public static bool IsHolliday(DateTime date)
        {
            bool Vec = false;
            List<string> DYear =  new List<string>();
            List<string> FYear = new List<string>();
            List<string> DMonth =  new List<string>();
            List<string> FMonth = new List<string>();
            List<string> DDays =  new List<string>();
            List<string> FDays = new List<string>();
            using SQLiteConnection c = new SQLiteConnection(EDTSource);
            c.Open();
            SQLiteCommand command;
            string query = "SELECT DYear,FYear,DMonth,FMonth,DDays,FDays FROM Vacance";
            command = new SQLiteCommand(query, c);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    DYear.Add(reader.GetString(0));
                    FYear.Add(reader.GetString(1));
                    DMonth.Add(reader.GetString(2));
                    FMonth.Add(reader.GetString(3));
                    DDays.Add(reader.GetString(4));
                    FDays.Add(reader.GetString(5));
                }
            }
            for (int i = 0; i < DDays.Count; i++)
            {
                DateTime ValD = new DateTime(Int32.Parse(DYear[i]), Int32.Parse(DMonth[i]), Int32.Parse(DDays[i]));
                DateTime ValF = new DateTime(Int32.Parse(FYear[i]),Int32.Parse(FMonth[i]),Int32.Parse(FDays[i]));
                if (date > ValD && date < ValF)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetDS(DateTime date)
        {
            List<string> DateSam =  new List<string>();
            List<string> Mat =  new List<string>();
            using SQLiteConnection c = new SQLiteConnection(EDTSource);
            c.Open();
            SQLiteCommand command;
            string query = "SELECT Mat,Date  FROM DSPlanning";
            command = new SQLiteCommand(query, c);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    DateSam.Add(reader.GetString(0));
                    Mat.Add(reader.GetString(1));
                }
            }
            for (int i = 0; i < DateSam.Count; i++)
            {
                if (DateSam[i] == date.ToString("dd/MM/yyyy"))
                {
                    return Mat[i];
                }
            }
            return "";
        }

        public static (string, string, string, string) GetQuizzImport(string question)
        {
            string reponse = "";
            string ImgRep = "";
            string ImgQuest = "";
            
            
            return (question,reponse, ImgRep, ImgQuest);
        }

        public record CollesResult(
            List<string> Nom,
            List<string> Salle,
            List<string> Matiere,
            List<int> Heure,
            List<int> Jours
        );
        public static CollesResult GetColles(DateTime dlundi)
        {
            string Dates = "";
            List<string> Nom =  new List<string>();
            List<int> heure = new List<int>();
            List<int> Jours =  new List<int>();
            List<string> Salle = new List<string>();
            List<string> Matiére =  new List<string>();
            int i = 0;
            using SQLiteConnection c = new SQLiteConnection(UserSource);
            c.Open();
            SQLiteCommand command;
            string query = "SELECT Date FROM Colles";
            command = new SQLiteCommand(query, c); 
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    DateTime val = Convert.ToDateTime(reader.GetString(0));
                    string val2 = $"{val}- {dlundi}";
                    if (dlundi == val)
                    {
                        Dates = val.ToString("yyyy-MM-dd 00:00:00");
                        break;  
                    }
                }
            }
            if (Dates != "")
            {
                query = $"SELECT Nom,heure,Jours,Salle,Matiére FROM Colles where Date = \"{Dates}\"";
                command = new SQLiteCommand(query, c);  
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Nom.Add(reader.GetString(0));
                        heure.Add(int.Parse(reader.GetString(1).Split("h")[0])-8);
                        Jours.Add(GetColumnNum(reader.GetString(2)));
                        Salle.Add(reader.GetString(3));
                        Matiére.Add(reader.GetString(4));
                        string val = $"{Nom[i]}-{heure[i]}-{Jours[i]}-{Salle[i]}-{Matiére[i]}";
                        i++;
                    }
                }
            }
            return new CollesResult(Nom, Salle, Matiére, heure, Jours);
        }

        public static string GetActualAlt(DateTime dlundi)
        {
            string Actual = "";
            using SQLiteConnection c = new SQLiteConnection(UserSource);
            c.Open();
            SQLiteCommand command;
            string query = "SELECT Physique FROM Alternance WHERE date = \"" +  dlundi.ToString("yyyy-MM-dd 00:00:00") + "\"";
            command = new SQLiteCommand(query, c); 
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                     Actual = reader.GetString(0);
                }
            }
            return Actual;
        }
        

        public static int GetColumnNum(string Seljour)
        {
            string[] Jours = ["lun", "mar", "mer", "jeu", "ven"];
            for (int i = 0; i < Jours.Length; i++)
            {
                if (Seljour == Jours[i])
                {
                    return i;
                }
            }
            return 6;
        }

        public static List<string> QuickSelect(string query,string Source )
        {
            List<string> Value = [];
            using SQLiteConnection c = new SQLiteConnection(Source);
            c.Open();
            SQLiteCommand command;
            command = new SQLiteCommand(query, c); 
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Value.Add(reader.GetString(0));
                }
            }
            return Value;
        }
        
        public static (List<string>,List<string>,List<int>,List<string>,List<string>) GetAllColle()
        {
            List<string> Nom =  new List<string>();
            List<DateTime> DatesT = new List<DateTime>();
            List<string> Dates =  new List<string>();
            List<int> heure = new List<int>();
            List<int> Jours =  new List<int>();
            List<string> Salle = new List<string>();
            List<string> Matiére =  new List<string>();
            int y = 0;
            using SQLiteConnection c = new SQLiteConnection(UserSource);
            c.Open();
            SQLiteCommand command;
            string query = $"SELECT * FROM Colles";
            command = new SQLiteCommand(query, c);  
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Nom.Add(reader.GetString(0));
                    DatesT.Add(Convert.ToDateTime(reader.GetString(1)));
                    heure.Add(int.Parse(reader.GetString(2).Split("h")[0])-8);
                    Jours.Add(GetColumnNum(reader.GetString(3)));
                    Salle.Add(reader.GetString(4));
                    Matiére.Add(reader.GetString(5));
                    string val = $"{Nom[y]}-{DatesT[y]}-{heure[y]}-{Jours[y]}-{Salle[y]}-{Matiére[y]}";
                    y++;
                }
            }
            for (int i = 0; i < DatesT.Count; i++)
            {
                Dates.Add((DatesT[i].AddDays(Jours[i])).ToString("yyyy-MM-dd"));
            }
            return (Nom,Dates,heure,Salle,Matiére);
        }
        
        // ############################### Fonction pour le stockage des quizz compléter
        public static void AddCompletion(string name,string mat)
        {
            try
            {
                using SQLiteConnection c = new SQLiteConnection(ConSource);
                c.Open();
                SQLiteCommand command;
                string query = $"UPDATE REFERENCE SET Ended = '1' WHERE Name = '{name}';";
                command = new SQLiteCommand(query, c);  
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show("An error occured while saving your completion : " + e); 
                throw;
            }
        }
    }
}
