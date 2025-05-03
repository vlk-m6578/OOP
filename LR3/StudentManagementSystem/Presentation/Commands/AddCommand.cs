using StudentManagementSystem.Application;
using StudentManagementSystem.Infrastructure.Api;
using StudentManagementSystem.Domain.DTOs;

namespace StudentManagementSystem.Presentation.Commands
{
    public class AddCommand : ICommand
    {
        private readonly IStudentService _studentService;
        private readonly IQuoteApiClient _quoteClient;

        public AddCommand(IStudentService studentService, IQuoteApiClient quoteClient)
        {
            _studentService = studentService;
            _quoteClient = quoteClient;
        }

        public async Task ExecuteAsync()
        {
            Console.Write("Enter student name: ");
            var name = Console.ReadLine();
            Console.Write("Enter student grade: ");
            var grade = int.Parse(Console.ReadLine());

            var dto = new StudentDTO { Name = name, Grade = grade };

            try
            {
                var student = _studentService.AddStudent(dto);
                Console.WriteLine($"Added  Student ID: {student.Id}");

                var quote = await _quoteClient.GetRandomQuoteAsync();
                Console.WriteLine($"Motivational Quote: \"{quote.Content}\" - {quote.Author}");
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
