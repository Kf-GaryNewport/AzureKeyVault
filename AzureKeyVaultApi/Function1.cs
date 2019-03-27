using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

using System.Configuration;
using Newtonsoft.Json;
using System.Text;



namespace AzureKeyVaultApi
{
    public static class Function1
    {
        private static string User = System.Environment.GetEnvironmentVariable("applicationuser");
        private static string Another = System.Environment.GetEnvironmentVariable("AnotherSecret");
        private static string fred = System.Environment.GetEnvironmentVariable("fred");

        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            log.Info($"Shhhhh.. it's a secret User: {User}");
            log.Info($"Shhhhh.. it's a secret Another: {Another}");
            log.Info($"Shhhhh.. it's a secret Fred: {fred}");

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            if (name == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                name = data?.name;
            }

            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }

        //[FunctionName("Function2")]
        //public static async Task<HttpResponseMessage> Run2([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        //{
        //    log.Info("C# HTTP trigger function processed a request.");

        //    //SecretRequest secretRequest = new SecretRequest();
        //    //secretRequest.Secret = "applicationUser";

        //    SecretRequest secretRequest = await req.Content.ReadAsAsync<SecretRequest>();

        //    if (string.IsNullOrEmpty(secretRequest.Secret))
        //        return req.CreateResponse(HttpStatusCode.BadRequest, "Request does not contain a valid Secret.");

        //    log.Info($"GetKeyVaultSecret request received for secret {secretRequest.Secret}");

        //    var serviceTokenProvider = new AzureServiceTokenProvider();
        //    log.Info($"PrincipalUsed {serviceTokenProvider.PrincipalUsed}");

        //    var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(serviceTokenProvider.KeyVaultTokenCallback));

        //    var secretUri = SecretUri(secretRequest.Secret);
        //    log.Info($"Key Vault URI {secretUri} generated");
        //    SecretBundle secretValue;
        //    try
        //    {
        //        secretValue = await keyVaultClient.GetSecretAsync(secretUri);
        //    }
        //    catch (KeyVaultErrorException kex)
        //    {
        //        return req.CreateResponse(HttpStatusCode.NotFound, $"{kex.Message}");
        //    }
        //    log.Info("Secret Value retrieved from KeyVault. {secretValue.Value}");

        //    var secretResponse = new SecretResponse { Secret = secretRequest.Secret, Value = secretValue.Value };

        //    return new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new StringContent(JsonConvert.SerializeObject(secretResponse), Encoding.UTF8, "application/json")
        //    };
        //}

        //public class SecretRequest
        //{
        //    public string Secret { get; set; }
        //}

        //public class SecretResponse
        //{
        //    public string Secret { get; set; }
        //    public string Value { get; set; }
        //}

        //public static string SecretUri(string secret)
        //{
        //    return $"{ConfigurationManager.AppSettings["KeyVaultUri"]}secrets/{secret}";
        //}
    }
}
