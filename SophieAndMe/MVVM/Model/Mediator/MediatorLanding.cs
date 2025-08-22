using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SophieAndMe.MVVM.Model;

public class MediatorLanding
{
    public class WebJsMessage
    {
        public string Meta { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        public string Positions { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
    
    public class JsCallMessage : ValueChangedMessage<string>
    {
        public JsCallMessage(string value) : base(value) { }
    }

    public class JsToAppMessage : ValueChangedMessage<(string Meta, string Data,string Position,string Notes)>
    {
        public JsToAppMessage(string Meta, string Data,string Positions,string Notes) : base((Meta,Data,Positions,Notes)) { }
    }
}