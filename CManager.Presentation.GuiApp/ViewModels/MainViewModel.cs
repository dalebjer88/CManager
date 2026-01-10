using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CManager.Presentation.GuiApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly CustomersListViewModel _customersViewModel;
    private readonly CreateCustomerViewModel _createViewModel;

    [ObservableProperty]
    private object? _currentViewModel;

    public MainViewModel(CustomersListViewModel customersViewModel, CreateCustomerViewModel createViewModel)
    {
        _customersViewModel = customersViewModel;
        _createViewModel = createViewModel;

        CurrentViewModel = _customersViewModel;
    }

    [RelayCommand]
    private void ShowCustomersView()
    {
        _customersViewModel.LoadCustomers();
        CurrentViewModel = _customersViewModel;
    }

    [RelayCommand]
    private void ShowCreateView() => CurrentViewModel = _createViewModel;
}