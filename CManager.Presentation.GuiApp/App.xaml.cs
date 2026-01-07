using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using CManager.Presentation.GuiApp.ViewModels;

namespace CManager.Presentation.GuiApp;

public partial class App : System.Windows.Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        services.AddTransient<MainViewModel>();

        _serviceProvider = services.BuildServiceProvider();

        var mainWindow = new MainWindow
        {
            DataContext = _serviceProvider.GetRequiredService<MainViewModel>()
        };

        mainWindow.Show();
    }
}
