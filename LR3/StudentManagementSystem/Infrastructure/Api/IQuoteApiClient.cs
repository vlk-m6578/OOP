using StudentManagementSystem.Domain.DTOs;

namespace StudentManagementSystem.Infrastructure.Api
{
    public interface IQuoteApiClient
    {
        Task<QuoteDTO> GetRandomQuoteAsync();
    }
}
