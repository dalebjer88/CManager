using CManager.Application.Interfaces;
using CManager.Domain.Models;

namespace CManager.Presentation.ConsoleApp.Controllers;

public class MenuController(ICustomerService customerService)
{
    private readonly ICustomerService _customerService = customerService;

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            ShowMenu();
            Console.Write("Choose option: ");
            var option = Console.ReadLine();

            Console.Clear();

            switch (option)
            {
                case "1":
                    CreateCustomer();
                    break;
                case "2":
                    ViewAllCustomers();
                    break;
                case "3":
                    ViewSpecificCustomer();
                    break;
                case "4":
                    DeleteCustomer();
                    break;
                case "0":
                    Console.WriteLine("Exiting application...");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Press ENTER to return to the menu...");
            Console.ReadLine();
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine("Customer Manager");
        Console.WriteLine();
        Console.WriteLine("1. Create customer");
        Console.WriteLine("2. View all customers");
        Console.WriteLine("3. View specific customer by email");
        Console.WriteLine("4. Delete customer by email");
        Console.WriteLine("0. Exit");
        Console.WriteLine();
    }

    private void CreateCustomer()
    {
        Console.WriteLine("Create customer");

        var firstName = InputRequired("First name");
        var lastName = InputRequired("Last name");
        var email = EmailRequired("Email");
        var phoneNumber = InputRequired("Phone number");
        var street = InputRequired("Street");
        var postalCode = InputRequired("Postal code");
        var city = InputRequired("City");

        var success = _customerService.CreateCustomer(
            firstName,
            lastName,
            email,
            phoneNumber,
            street,
            postalCode,
            city);

        Console.WriteLine();

        if (!success)
        {
            Console.WriteLine("Customer could not be created.");
            Console.WriteLine("Check input values or if the email already exists.");
            return;
        }

        var customer = _customerService.GetCustomerByEmail(email);

        Console.WriteLine("Customer created:");

        if (customer == null)
        {
            Console.WriteLine("Customer was created, but could not be loaded for display.");
            return;
        }

        Console.WriteLine($"Id: {customer.Id}");
        Console.WriteLine($"Name: {customer.FullName}");
        Console.WriteLine($"Email: {customer.Email}");
    }

    private string InputRequired(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");
            var input = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(input))
                return input;

            Console.WriteLine($"{label} is required. Please enter a value.");
        }
    }

    private string EmailRequired(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");
            var input = Console.ReadLine()?.Trim() ?? string.Empty;

            if (IsValidEmail(input))
                return input;

            Console.WriteLine("Please enter a valid email address.");
        }
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return email.Contains('@') && email.Contains('.') && email.Length >= 5;
    }

    private void ViewAllCustomers()
    {
        Console.WriteLine("All customers");

        List<Customer> customers = _customerService.GetAllCustomers();

        if (customers.Count == 0)
        {
            Console.WriteLine("No customers found.");
            return;
        }

        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.FullName} - {customer.Email}");
        }
    }

    private void ViewSpecificCustomer()
    {
        Console.WriteLine("View specific customer");

        var email = EmailRequired("Enter email");

        var customer = _customerService.GetCustomerByEmail(email);

        if (customer == null)
        {
            Console.WriteLine("No customer found with that email.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Id: {customer.Id}");
        Console.WriteLine($"Name: {customer.FullName}");
        Console.WriteLine($"Email: {customer.Email}");
        Console.WriteLine($"Phone: {customer.PhoneNumber}");
        Console.WriteLine($"Street: {customer.Address.Street}");
        Console.WriteLine($"Postal code: {customer.Address.PostalCode}");
        Console.WriteLine($"City: {customer.Address.City}");
    }

    private void DeleteCustomer()
    {
        Console.WriteLine("Delete customer");

        var email = EmailRequired("Enter email");

        var customer = _customerService.GetCustomerByEmail(email);

        if (customer == null)
        {
            Console.WriteLine("No customer found with that email.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("You have selected:");
        Console.WriteLine($"Name: {customer.FullName}");
        Console.WriteLine($"Email: {customer.Email}");
        Console.WriteLine();
        Console.Write("Are you sure you want to delete this customer? (y/n): ");

        var confirmation = (Console.ReadLine() ?? string.Empty).Trim().ToLower();

        if (confirmation != "y")
        {
            Console.WriteLine("Delete cancelled.");
            return;
        }

        var success = _customerService.DeleteCustomerByEmail(email);

        Console.WriteLine(success ? "Customer deleted." : "Something went wrong.");
    }
}
