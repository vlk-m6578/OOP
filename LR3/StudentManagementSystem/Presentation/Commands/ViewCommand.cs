using StudentManagementSystem.Application;
using StudentManagementSystem.Domain.DTOs;

namespace StudentManagementSystem.Presentation.Commands
{
    public class ViewCommand : ICommand
    {
        private readonly IStudentService _studentService;

        public ViewCommand(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task ExecuteAsync()
        {
            var students = _studentService.GetAllStudents();
            Console.WriteLine("\nStudents: ");
            foreach (var s in students)
            {
                Console.WriteLine($"{s.Id}: {s.Name} - Grade: {s.Grade}");
            }
        }
    }
}
