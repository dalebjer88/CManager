using CManager.Application.Factories;
using CManager.Application.Helpers;
using CManager.Application.Interfaces;
using CManager.Application.Services;
using CManager.Domain.Interfaces;
using CManager.Infrastructure.Formatters;
using CManager.Infrastructure.Repos;
using CManager.Presentation.GuiApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace CManager.Presentation.GuiApp;

public partial class App : System.Windows.Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        var services = new ServiceCollection();

        var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CManager");
        var filePath = Path.Combine(folderPath, "customers.json");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        services.AddSingleton<IIdGenerator, IdGenerator>();
        services.AddSingleton<ICustomerFactory, CustomerFactory>();
        services.AddSingleton<IJsonFormatter, JsonFormatter>();

        services.AddSingleton<ICustomerRepo>(sp =>
            new CustomerRepo(filePath, sp.GetRequiredService<IJsonFormatter>()));

        services.AddSingleton<ICustomerService, CustomerService>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<CustomersListViewModel>();
        services.AddSingleton<CreateCustomerViewModel>();

        services.AddSingleton<MainWindow>(sp => new MainWindow
        {
            DataContext = sp.GetRequiredService<MainViewModel>()
        });

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        base.OnStartup(e);
    }
}