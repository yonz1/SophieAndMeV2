using EAGetMail;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SophieAndMe.MVVM.Model;

public class MailRepository
{
    static string[] Scopes = { GmailService.Scope.GmailReadonly };
    static string ApplicationName = "Gmail API .NET Demo";

    public static async Task Main()
    {
        Console.WriteLine("Debug1");
        UserCredential credential;

        using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            string credPath = "token.json";
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true));
        }

        Console.WriteLine("Debug1");
        // Créer le service Gmail
        var service = new GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        // Récupérer la liste des messages
        var request = service.Users.Messages.List("me");
        request.MaxResults = 5; // on prend les 5 derniers mails
        var messagesResponse = await request.ExecuteAsync();
        Console.WriteLine("Debug2");
        if (messagesResponse.Messages != null)
        {
            foreach (var msg in messagesResponse.Messages)
            {
                var emailInfoReq = service.Users.Messages.Get("me", msg.Id);
                var emailInfoResponse = await emailInfoReq.ExecuteAsync();

                string subject = "";
                string from = "";

                foreach (var header in emailInfoResponse.Payload.Headers)
                {
                    if (header.Name == "Subject")
                        subject = header.Value;
                    else if (header.Name == "From")
                        from = header.Value;
                }

                Console.WriteLine($"De: {from}");
                Console.WriteLine($"Sujet: {subject}");
                Console.WriteLine(new string('-', 40));
            }
        }
    }
}