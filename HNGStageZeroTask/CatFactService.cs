using System.Text.Json;

namespace HNGStageZeroTask
{
    public interface ICatFactService
    {
        Task<string> GetRandomCatFactAsync();
    }

    public class CatFactService : ICatFactService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CatFactService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetRandomCatFactAsync()
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync("https://catfact.ninja/fact");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                return doc.RootElement.GetProperty("fact").GetString();
            }
            catch
            {
                return "Cats are wonderful animals, but our cat fact service is currently unavailable.";
            }
        }
    }
}
