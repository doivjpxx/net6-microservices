namespace Play.Inventory.Service.Clients;

public class CatalogClient
{
    private readonly HttpClient _httpClient;

    public CatalogClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>($"/Items");
        return response;
    }
}