
using System.Data.SQLite;
using System.Windows.Forms;
using SophieAndMe.Core;
using SophieAndMe.MVVM.View;


namespace SophieAndMe.MVVM.Model
{
    class DBInteraction
    {
        
        // ################################################### Initialisations 
        
        private static readonly List<string> Aquestion = new List<string>();
        private static readonly List<string> Arepnse = new List<string>();
        private static readonly List<string> AurlQuestion = new List<string>();
        private static readonly List<string> AurlRep = new List<string>();
        private static readonly string ConSource = "Data Source=..//..//..//Database//data_restored.db";

        // ################################################### Fonctions
        
        
        public static void MarkData(string question, string repnse, string urlQuestion, string urlRep)
        {
            string query = "";
            using (SQLiteConnection c = new SQLiteConnection(ConSource))
            {
                c.Open();
                query = "SELECT COUNT(*) FROM Marked WHERE  question = \"" + question + "\" AND Matier =   \"" + App.Current.Properties["matier"].ToString() + "\"";
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
                        query = "INSERT INTO Marked (Matier,question,reponse,question_img,reponse_img) VALUES (" + mat + quest + rep + questionImg + reponseImg + ")";
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
                string query = "DELETE FROM Marked WHERE question = \"" + question +"\"";
                Console.WriteLine(query);
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
                    using(var reader = cmd.ExecuteReader())
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
                    using(var reader = cmd.ExecuteReader())
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
                result = ["Mathématiques", "Physique", "SI", "Français", "Anglais", "Erreurs"];
            }
            return result;
        }
        
        public static (List<string>,List<string>,List<string>,List<string>) Retrievequizz(string? nameindex)
        {
            var connection = new SQLiteConnection(ConSource);
            string query = "";
            if ((string)App.Current.Properties["matier"] == "All")
            {
                query = "SELECT question,reponse,image_question_url,image_answer_url FROM " + nameindex;
                App.Current.Properties["matier"] = nameindex;
            }
            else if (nameindex.Contains("Marked"))
            {
                query = "SELECT question,reponse,image_question_url,image_answer_url FROM Marked Where Matier = \"" + App.Current.Properties["matier"].ToString() + "\"" ;
            }
            else 
            {
                query = "SELECT question,reponse,image_question_url,image_answer_url  FROM " + App.Current.Properties["matier"].ToString() + " WHERE name = \"" + nameindex + "\"";
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
                    NavigationService.Instance.Navigate(new VMarked());    
                }
                else
                {
                    NavigationService.Instance.Navigate(new VQuizz());
                }
            }
            
            return (Aquestion,Arepnse,AurlQuestion,AurlRep);
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
                query = "SELECT " + valtoreq + " FROM Marked where Matier = \"" + mat  + "\"" ;
            }
            
            using (var db = new SQLiteConnection(ConSource))
            {
                db.Open();
                using (var cmd = new SQLiteCommand(query, db)) 
                using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                         question.Add(reader.GetString(0));
                         rep.Add(reader.GetString(1));
                         urlRep.Add(reader.GetString(2));
                         urlQuestion.Add(reader.GetString(3));
                    }
                }
            }
            return (question,rep,urlQuestion,urlRep);
        }
        
        public static List<string> GetMarkedForQUizz(string mat)
        {
            List<string> question = new List<string>();
            string valtoreq = "question";
            string query = "";
            if (mat == "All")
            {
                query = "SELECT " + valtoreq + " FROM Marked where Matier = \"" + App.Current.Properties["nameindex"].ToString() + "\"" ;
                Console.WriteLine("All query : " + query);
            }
            else
            {
                query = "SELECT " + valtoreq + " FROM Marked where Matier = \"" + mat  + "\"" ;
            }
            
            using (var db = new SQLiteConnection(ConSource))
            {
                db.Open();
                using (var cmd = new SQLiteCommand(query, db)) 
                using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        question.Add(reader.GetString(0));
                    }
                }
            }
            return question;
        }
    }
}
