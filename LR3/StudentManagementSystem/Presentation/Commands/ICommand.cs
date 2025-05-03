using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Presentation.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }
}
