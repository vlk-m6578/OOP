using StudentManagementSystem.Domain.DTOs;
using StudentManagementSystem.Domain.Models;

namespace StudentManagementSystem.Application
{
    public interface IStudentService
    {
        Student AddStudent(StudentDTO studentDTO);
        void UpdateStudent(int id, StudentDTO studentDTO);
        IEnumerable<Student> GetAllStudents();
        Student GetStudent(int id);
    }
}
