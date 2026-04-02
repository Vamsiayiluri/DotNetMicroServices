using System.Net.Http.Headers;


namespace OrderService.Infrastructure.Services
{
    public class ProductClient
    {
        private readonly HttpClient _httpClient;

        public ProductClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ProductExists(Guid productId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(
                $"https://localhost:7066/api/products");

            return response.IsSuccessStatusCode;
        }
    }
}
