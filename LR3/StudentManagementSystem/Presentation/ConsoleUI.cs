using StudentManagementSystem.Presentation.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Presentation
{
    public class ConsoleUI
    {
        private readonly CommandInvoker _invoker;

        public ConsoleUI(CommandInvoker invoker)
        {
            _invoker = invoker;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.WriteLine("\n1. Add Student\n2. Edit Student\n3. View Student\n4. Exit");
                Console.Write("Choose option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await _invoker.ExecuteCommand("add");
                        break;
                    case "2":
                        await _invoker.ExecuteCommand("edit");
                        break;
                    case "3":
                        await _invoker.ExecuteCommand("view");
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}
