using Microsoft.Extensions.DependencyInjection;
using StudentManagementSystem.DataAccess.Repositories;
using StudentManagementSystem.Application;
using StudentManagementSystem.Infrastructure.Api;
using StudentManagementSystem.Presentation;
using StudentManagementSystem.Presentation.Commands;

class Program
{
    static async Task Main()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IStudentRepository, StudentRepository>();
        services.AddHttpClient<IQuoteApiClient, QuoteApiClient>()
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            });
        services.AddSingleton<IStudentService, StudentService>();
        services.AddTransient<AddCommand>();
        services.AddTransient<EditCommand>();
        services.AddTransient<ViewCommand>();

        var provider = services.BuildServiceProvider();

        // Настройка команд
        var commands = new Dictionary<string, ICommand>
        {
            ["add"] = provider.GetRequiredService<AddCommand>(),
            ["edit"] = provider.GetRequiredService<EditCommand>(),
            ["view"] = provider.GetRequiredService<ViewCommand>()
        };

        var invoker = new CommandInvoker(commands);
        var ui = new ConsoleUI(invoker);

        await ui.RunAsync();
    }
}
