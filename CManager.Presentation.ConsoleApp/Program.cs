using CManager.Application.Helpers;
using CManager.Application.Interfaces;
using CManager.Application.Services;
using CManager.Application.Factories;
using CManager.Domain.Interfaces;
using CManager.Infrastructure.Formatters;
using CManager.Infrastructure.Repos;
using CManager.Presentation.ConsoleApp.Controllers;

namespace CManager.Presentation.ConsoleApp;

internal class Program
{
    private static void Main(string[] args)
    {
        var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CManager");
        var filePath = Path.Combine(folderPath, "customers.json");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        IJsonFormatter formatter = new JsonFormatter();
        ICustomerRepo repo = new CustomerRepo(filePath, formatter);

        IIdGenerator idGenerator = new IdGenerator();
        ICustomerFactory customerFactory = new CustomerFactory();

        ICustomerService service = new CustomerService(repo, idGenerator, customerFactory);

        var menu = new MenuController(service);
        menu.Run();
    }
}