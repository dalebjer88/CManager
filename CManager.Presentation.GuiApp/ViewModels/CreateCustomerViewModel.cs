using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CManager.Application.Interfaces;

namespace CManager.Presentation.GuiApp.ViewModels;

public partial class CreateCustomerViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;

    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _phoneNumber = string.Empty;

    [ObservableProperty]
    private string _street = string.Empty;

    [ObservableProperty]
    private string _postalCode = string.Empty;

    [ObservableProperty]
    private string _city = string.Empty;

    public CreateCustomerViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [RelayCommand]
    private void Save()
    {
        var result = _customerService.CreateCustomer(
            FirstName, LastName, Email, PhoneNumber, Street, PostalCode, City);

        var activeWindow = System.Windows.Application.Current.MainWindow;

        if (result)
        {
            System.Windows.MessageBox.Show(
                activeWindow,
                "Customer has been successfully created!",
                "Success",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);

            FirstName = LastName = Email = PhoneNumber = Street = PostalCode = City = string.Empty;
        }
        else
        {
            System.Windows.MessageBox.Show(
                activeWindow,
                "Failed to create customer. Please make sure the email is unique and all fields are correct.",
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }
}