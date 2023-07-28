using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using System.Threading;

namespace Institute.DAOs.Services
{


    public class GmailOAuth2
    {
        public static async Task<string> GetAccessToken(string clientId, string clientSecret, string user)
        {
            UserCredential credential;
            using (var stream = new FileStream("appsettings.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { GmailService.Scope.GmailSend },
                    user,
                    CancellationToken.None
                );
            }

            return credential.Token.AccessToken;
        }
    }

}
