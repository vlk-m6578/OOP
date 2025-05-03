using StudentManagementSystem.Domain.Models;
using System.Text.Json;

namespace StudentManagementSystem.DataAccess.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private const string FilePath = "students.json";
        private List<Student> _students;

        public StudentRepository() 
        {
            LoadStudents();
        }

        private void LoadStudents()
        {
            if(File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                _students = JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
            }
            else
            {
                _students = new List<Student>();
            }
        }

        private void SaveStudents()
        {
            var json = JsonSerializer.Serialize(_students);
            File.WriteAllText(FilePath, json);
        }

        public IEnumerable<Student> GetAll() => _students;

        public Student Add(Student student)
        {
            student.Id = GetNextId();
            _students.Add(student);
            SaveStudents();
            return student;
        }

        public void Update(Student student)
        {
            var index = _students.FindIndex(s => s.Id == student.Id);
            if(index != -1)
            {
                _students[index] = student;
                SaveStudents();
            }
        }

        public Student GetById(int id) => _students.FirstOrDefault(s => s.Id == id);

        public int GetNextId() => _students.Any() ? _students.Max(s => s.Id) + 1 : 1;
    }
}
