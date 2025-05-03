using StudentManagementSystem.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Domain.Validators
{
    public static class StudentValidator
    {
        public static void Validate(StudentDTO studentDTO)
        {
            if (string.IsNullOrWhiteSpace(studentDTO.Name))
                throw new ArgumentException("Name can't be empty.");
            if (studentDTO.Grade < 0 || studentDTO.Grade > 100)
                throw new ArgumentException("Grade must be between 0 and 100.");
        }
    }
}
