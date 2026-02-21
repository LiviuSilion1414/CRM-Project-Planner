using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using RestSharp; // RestSharp v112.1.0
using RestSharp.Authenticators;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace MailGunExamples
{

    public class MailSender
    {
        public async Task<RestResponse> Send()
        {
            var options = new RestClientOptions("https://api.mailgun.net")
            {
                Authenticator = new HttpBasicAuthenticator("api", Environment.GetEnvironmentVariable("API_KEY") ?? "a6357f76035e487a7460ad9fb0f8a6cc-5ba06dbe-50f5a3f8")
            };

            var client = new RestClient(options);
            var request = new RestRequest("/v3/sandbox94ad01beb2d44e678a156adff61d64f9.mailgun.org/messages", RestSharp.Method.Post);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox94ad01beb2d44e678a156adff61d64f9.mailgun.org>");
            request.AddParameter("to", "Liviu Maricel Silion <silionliviu001@gmail.com>");
            request.AddParameter("subject", "Hello Liviu Maricel Silion");
            request.AddParameter("text", "Congratulations Liviu Maricel Silion, you just sent an email with Mailgun! You are truly awesome!");
            return await client.ExecuteAsync(request);
        }
    }
}
//a6357f76035e487a7460ad9fb0f8a6cc-5ba06dbe-50f5a3f8
//sandbox94ad01beb2d44e678a156adff61d64f9.mailgun.org
//https://api.mailgun.net