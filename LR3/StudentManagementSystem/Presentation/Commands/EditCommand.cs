using StudentManagementSystem.Application;
using StudentManagementSystem.Domain.DTOs;

namespace StudentManagementSystem.Presentation.Commands
{
    public class EditCommand : ICommand
    {
        private readonly IStudentService _studentService;

        public EditCommand(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task ExecuteAsync()
        {
            Console.Write("Enter student ID to edit: ");
            var id = int.Parse(Console.ReadLine());

            var student = _studentService.GetStudent(id);
            if(student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.Write($"New name ({student.Name}): ");
            var name = Console.ReadLine();
            Console.Write($"New grade ({student.Grade}):");
            var grade = int.Parse(Console.ReadLine());

            var dto = new StudentDTO
            {
                Name = string.IsNullOrEmpty(name) ? student.Name : name,
                Grade = grade
            };

            try
            {
                _studentService.UpdateStudent(id, dto);
                Console.WriteLine("Student updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
