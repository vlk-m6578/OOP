using StudentManagementSystem.Domain.DTOs;

namespace StudentManagementSystem.Domain.Factories
{
    public static class QuoteFactory
    {
        public static QuoteDTO CreateQuote(string content, string author)
        {
            return new QuoteDTO
            {
                Content = content,
                Author = author
            };
        }
    }
}
