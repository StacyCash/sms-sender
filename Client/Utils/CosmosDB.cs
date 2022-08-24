using Microsoft.Azure.Cosmos;

namespace Client.Utils
{
    public class CosmosDBUtils
    {
        public static Container GetDBContainer(IConfiguration configuration)
        {
          ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

            string connectionString = configuration["VonageAppDbConnectionString"];
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(
                    $"Configuration does not contain a connection string: {nameof(configuration)}");
            }  
            
            CosmosClient client = new CosmosClient(connectionString);
            return client.GetContainer(configuration["VonageAppDBName"], "VonageApp");
        }
    }
}
