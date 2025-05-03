using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Presentation.Commands
{
    public class CommandInvoker
    {
        private readonly Dictionary<string, ICommand> _commands;

        public CommandInvoker(Dictionary<string, ICommand> commands)
        {
            _commands = commands;
        }

        public async Task ExecuteCommand(string commandKey)
        {
            if (_commands.TryGetValue(commandKey, out var command))
                await command.ExecuteAsync();
            else
                Console.WriteLine("Invalid Command.");
        }
    }
}
