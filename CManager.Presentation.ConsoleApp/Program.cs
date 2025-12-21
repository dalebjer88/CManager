using CManager.Application.Helpers;
using CManager.Application.Interfaces;
using CManager.Application.Services;
using CManager.Domain.Interfaces;
using CManager.Infrastructure.Formatters;
using CManager.Infrastructure.Repos;
using CManager.Presentation.ConsoleApp.Controllers;

namespace CManager.Presentation.ConsoleApp;

internal class Program
{
    private static void Main(string[] args)
    {
        var filePath = "customers.json";

        IJsonFormatter formatter = new JsonFormatter();
        ICustomerRepo repo = new CustomerRepo(filePath, formatter);
        IIdGenerator idGenerator = new IdGenerator();

        ICustomerService service = new CustomerService(repo, idGenerator);

        var menu = new MenuController(service);
        menu.Run();
    }
}