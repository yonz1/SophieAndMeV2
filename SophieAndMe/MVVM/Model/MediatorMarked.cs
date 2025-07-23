using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SophieAndMe.Core;

public class MediatorMarked
{
    public class WebJsMessage
    {
        public string action { get; set; } = string.Empty;
        public string question { get; set; } = string.Empty;
    }
    
    public class JsCallMessage : ValueChangedMessage<string>
    {
        public JsCallMessage(string value) : base(value) { }
    }

    public class JsToAppMessage : ValueChangedMessage<(string Action, string Question)>
    {
        public JsToAppMessage(string action, string question) : base((action, question)) { }
    }
}