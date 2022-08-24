using Newtonsoft.Json;

namespace Models.Application;

public class GetApplicationsResponse
{
    [JsonProperty("page_size")]
    public int PageSize { get; set; }
    
    public int Page { get; set; }
    
    [JsonProperty("total_items")]
    public int TotalItems { get; set; }
    
    [JsonProperty("total_pages")]
    public int TotalPages { get; set; }

    [JsonProperty("_embedded")]
    public Embedded Embedded { get; set; }

    [JsonProperty("_links")]
    public Links Links { get; set; }
}