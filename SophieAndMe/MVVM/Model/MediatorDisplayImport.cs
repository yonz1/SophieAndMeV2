using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SophieAndMe.MVVM.Model;

public class MediatorDisplayImport
{
    //################################################################################ Type de message reçus

    
    public class WebJsMessage
    {
        public  string action  { get; set; } = string.Empty;
        public string matier { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string question { get; set; } = string.Empty;
        public string imgQuestion { get; set; } = string.Empty;
        public string rep { get; set; } = string.Empty;
        public string imgRep { get; set; } = string.Empty;
    }
    
    
    // ################################################################################## Fonctions de reception des messages
    
    public class  JstoAppMessage : ValueChangedMessage<(string Action, string Matier, string Name,string Question,string ImgQuestion,string Rep,string ImgRep)>
    {
        public JstoAppMessage(string action, string matier, string name, string question, string imgQuestion, string rep, string imgRep)  : base((action, matier, name, question, imgQuestion, rep, imgRep)) {}
    }
    
    //################################################################################# Fonction d'apelle des messsages
    
    public class JsCallMessage : ValueChangedMessage<string>
    {
        public JsCallMessage(string value) : base(value) { }
    }
}