using StudentManagementSystem.DataAccess.Repositories;
using StudentManagementSystem.Domain.Models;
using StudentManagementSystem.Domain.DTOs;
using StudentManagementSystem.Domain.Validators;
using StudentManagementSystem.Domain.Factories;

namespace StudentManagementSystem.Application
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public Student AddStudent(StudentDTO studentDTO)
        {
            StudentValidator.Validate(studentDTO);
            var student = StudentFactory.CreateStudent(_repository.GetNextId(), studentDTO);
            return _repository.Add(student);
        }

        public void UpdateStudent(int id, StudentDTO studentDTO)
        {
            StudentValidator.Validate(studentDTO);
            var existing = _repository.GetById(id) ?? throw new ArgumentException("Student not found.");
            var updated = StudentFactory.CreateStudent(id, studentDTO);
            _repository.Update(updated);
        }

        public IEnumerable<Student> GetAllStudents() => _repository.GetAll();

        public Student GetStudent(int id) => _repository.GetById(id);
    }
}
