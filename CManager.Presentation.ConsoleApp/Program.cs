using CManager.Application.Interfaces;
using CManager.Application.Services;
using CManager.Domain.Interfaces;
using CManager.Infrastructure.Repositories;
using CManager.Presentation.ConsoleApp.Controllers;

namespace CManager.Presentation.ConsoleApp;

internal class Program
{
    private static void Main(string[] args)
    {
        var filePath = "customers.json";

        ICustomerRepo customerRepo = new CustomerRepo(filePath);
        ICustomerService customerService = new CustomerService(customerRepo);
        var menuController = new MenuController(customerService);

        menuController.Run();
    }
}
