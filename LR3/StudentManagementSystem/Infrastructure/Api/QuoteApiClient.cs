using StudentManagementSystem.Domain.DTOs;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace StudentManagementSystem.Infrastructure.Api
{
    public class QuoteApiClient : IQuoteApiClient
    {
        private readonly HttpClient _httpClient;

        public QuoteApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<QuoteDTO> GetRandomQuoteAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://api.quotable.io/random");
                dynamic data = JsonConvert.DeserializeObject(response);
                return new QuoteDTO
                {
                    Content = data.content,
                    Author = data.author
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");

                return new QuoteDTO { Content = "No quote available", Author = "System" };
            }
        }
    }
}
