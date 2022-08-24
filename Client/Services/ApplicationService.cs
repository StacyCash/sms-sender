using Client.Utils;
using Microsoft.Azure.Cosmos;
using Models.Application;
using VonageCalls.Application;
using VonageCalls.Interfaces;

namespace Client.Services
{
    public class ApplicationService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IVonageApiKey _apiKey;

        public ApplicationService(IVonageApiKey apiKey, IConfiguration configuration, HttpClient httpClient)
        {
            ArgumentNullException.ThrowIfNull(apiKey, nameof(apiKey));
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
            ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
            
            _apiKey = apiKey;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<GetApplicationsResponse> GetApps()
        {
            VonageApplication vonageApplication = new VonageApplication(new HttpClient(), _apiKey);
            return await vonageApplication.GetApplicationsAsync();
        }

        public async Task<VonageApp> GetApp(Guid applicationId)
        {
            VonageApplication vonageApplication = new VonageApplication(new HttpClient(), _apiKey);
            return await vonageApplication.GetApplicationAsync(applicationId);
        }

        public async Task<VonageApp> CreateApp(VonageApp app)
        {
            VonageApplication vonageApplication = new VonageApplication(_httpClient, _apiKey);
            var createAppRequest = AdaptVonageAppRequest(app);
            var createdApp = await vonageApplication.CreateAppAsync(createAppRequest);

            var newApp = new
            {
                id = createdApp.Id,
                privateKey = createdApp.Keys.PrivateKey,
                userId = "Stacy"
            };

            var containerClient = CosmosDBUtils.GetDBContainer(_configuration);
            await containerClient.CreateItemAsync<dynamic>(newApp);

            return createdApp;
        }

        public async Task<VonageApp> UpdateApp(VonageApp appToUpdate)
        {
            var containerClient = CosmosDBUtils.GetDBContainer(_configuration);
            if (!await AppExists(containerClient, Guid.Parse(appToUpdate.Id), "Stacy"))
            {
                throw new KeyNotFoundException($@"Application with ID: {appToUpdate.Id}");
            }
            
            VonageApplication vonageApplication = new VonageApplication(_httpClient, _apiKey);
            var updateAppRequest = AdaptVonageAppRequest(appToUpdate);
            VonageApp updatedApp = await vonageApplication.UpdateAppAsync(updateAppRequest, Guid.Parse(appToUpdate.Id));

            return updatedApp;
        }

        public async Task DeleteApp(Guid id)
        {
            VonageApplication vonageApplication = new VonageApplication(_httpClient, _apiKey);

            await vonageApplication.DeleteApplicationAsync(id);

            var containerClient = CosmosDBUtils.GetDBContainer(_configuration);
            if (!await AppExists(containerClient, id, "Stacy"))
            {
                await containerClient.DeleteItemAsync<dynamic>(id.ToString(), new PartitionKey("Stacy"));
            }            
        }

        public async Task<bool> KnownByApplication(Guid id)
        {
            var containerClient = CosmosDBUtils.GetDBContainer(_configuration);
            return await AppExists(containerClient, id, "Stacy");
        }

        private static VonageAppRequest AdaptVonageAppRequest(VonageApp appToUpdate)
        {
            var updateAppRequest = new VonageAppRequest
            {
                Capabilities = new Capabilities
                {
                    Messages = new Messages
                    {
                        authenticate_inbound_media = appToUpdate.Capabilities.Messages.authenticate_inbound_media,
                        Version = appToUpdate.Capabilities.Messages.Version,
                        webhooks = new Webhooks
                        {
                            InboundUrl = new VonageUrl
                            {
                                Address = appToUpdate.Capabilities.Messages.webhooks.InboundUrl.Address,
                                HttpMethod = appToUpdate.Capabilities.Messages.webhooks.InboundUrl.HttpMethod
                            },
                            StatusUrl = new VonageUrl
                            {
                                Address = appToUpdate.Capabilities.Messages.webhooks.StatusUrl.Address,
                                HttpMethod = appToUpdate.Capabilities.Messages.webhooks.StatusUrl.HttpMethod
                            }
                        }
                    }
                },
                Keys = new Keys
                {
                    PublicKey = appToUpdate.Keys.PublicKey
                },
                Privacy = appToUpdate.Privacy,
                Name = $"{appToUpdate.Name} - Test"
            };
            return updateAppRequest;
        }

        private static async Task<bool> AppExists(Container container, Guid id, string userId)
        {
            QueryDefinition query = new QueryDefinition(
                    @"select
                    t.id
                  from t
                  where 
                    t.userId = @userId 
                    and t.id = @id")
                .WithParameter("@userId", userId)
                .WithParameter("@id", id.ToString());
            var todosIterator = container.GetItemQueryIterator<dynamic>(query, null, new QueryRequestOptions());

            List<dynamic> results = new List<dynamic>();

            while (todosIterator.HasMoreResults)
            {
                results.AddRange(await todosIterator.ReadNextAsync());
            }

            return results.Count > 0;
        }
    }
}
