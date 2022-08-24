using System.Net.Http.Headers;
using Client.Model;
using Client.Utils;
using Microsoft.Azure.Cosmos;
using Models.Messages;
using VonageCalls.Messages;
using VongaeJwt;

namespace Client.Services
{
    public class MessageService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public MessageService(IConfiguration configuration, HttpClient httpClient)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
            ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));

            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task SendMessage(string to, string message, Guid applicationId)
        {
            var containerClient = CosmosDBUtils.GetDBContainer(_configuration);
            string privateKey = await GetPrivateKey(containerClient, applicationId, "Stacy");

            if (string.IsNullOrWhiteSpace(privateKey))
            {
                throw new Exception("Coundn't retrieve private key");
            }

            string jwt = Jwt.CreateToken(applicationId.ToString(), privateKey);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var smsMessage = new SmsMessage
            {
                From = "+14157386102",
                To = to,
                Message = message
            };

            await new VonageMessagingService(_httpClient).SendMessage(smsMessage);
        }

        private static async Task<string> GetPrivateKey(Container container, Guid id, string userId)
        {
            QueryDefinition query = new QueryDefinition(
                    @"select
                    t.id,
                    t.privateKey
                  from t
                  where 
                    t.userId = @userId 
                    and t.id = @id")
                .WithParameter("@userId", userId)
                .WithParameter("@id", id.ToString());
            var appIterator = container.GetItemQueryIterator<VonageCosmosApp>(query, null, new QueryRequestOptions());
            List<VonageCosmosApp> privateKeys = new List<VonageCosmosApp>();

            if (appIterator.HasMoreResults)
            {
                privateKeys.AddRange(await appIterator.ReadNextAsync());
            }

            if (privateKeys.Count > 0)
            {
                return privateKeys.First().PrivateKey;
            }

            return string.Empty;
        }
    }
}
