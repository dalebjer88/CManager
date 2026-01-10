using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CManager.Application.Interfaces;
using CManager.Domain.Models;

namespace CManager.Presentation.GuiApp.ViewModels;

public partial class CustomersListViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;

    [ObservableProperty]
    private ObservableCollection<Customer> _customers = [];

    public CustomersListViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
        LoadCustomers();
    }

    public void LoadCustomers()
    {
        var result = _customerService.GetAllCustomers();
        Customers = new ObservableCollection<Customer>(result);
    }
}