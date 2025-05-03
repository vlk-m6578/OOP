using StudentManagementSystem.Domain.DTOs;
using StudentManagementSystem.Domain.Models;

namespace StudentManagementSystem.Domain.Factories
{
    public static class StudentFactory
    {
        public static Student CreateStudent(int id, StudentDTO dto)
        {
            return new Student
            {
                Id = id,
                Name = dto.Name,
                Grade = dto.Grade
            };
        }
    }
}
