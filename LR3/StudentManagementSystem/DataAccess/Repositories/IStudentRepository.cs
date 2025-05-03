using StudentManagementSystem.Domain.Models;

namespace StudentManagementSystem.DataAccess.Repositories
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetAll();
        Student Add(Student student);
        void Update(Student student);
        Student GetById(int id);
        int GetNextId();
    }
}
