using StudentManagementSystem.Domain.DTOs;

namespace StudentManagementSystem.Infrastructure.Api
{
    public class QuoteApiAdapter
    {
        private readonly IQuoteApiClient _client;

        public QuoteApiAdapter(IQuoteApiClient client)
        {
            _client = client;
        }
        public async Task<QuoteDTO> FetchMotivationalQuoteAsync()
        {
            return await _client.GetRandomQuoteAsync();
        }
    }
}
