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
                    DeleteCustomerByEmail();
                    break;

                case "4":
                    ViewCustomerByNumber();
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
        Console.WriteLine("3. Delete customer by email");
        Console.WriteLine("4. View specific customer");
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

    private void ViewAllCustomers()
    {
        Console.WriteLine("All customers");
        Console.WriteLine("");

        var customers = _customerService.GetAllCustomers().ToList();

        if (customers.Count == 0)
        {
            Console.WriteLine("No customers found.");
            return;
        }

        for (int i = 0; i < customers.Count; i++)
        {
            var customer = customers[i];
            Console.WriteLine($"{i + 1}. {customer.FullName} - {customer.Email}");
        }
    }

    private void DeleteCustomerByEmail()
    {
        Console.WriteLine("Delete customer");

        var email = EmailRequired("Enter email");

        var customer = _customerService.GetCustomerByEmail(email);

        if (customer == null)
        {
            Console.WriteLine("No customer found with that email.");
            return;
        }

        Console.WriteLine("You have selected:");
        Console.WriteLine($"Name: {customer.FullName}");
        Console.WriteLine($"Email: {customer.Email}");
        Console.WriteLine();

        if (!ConfirmYesNo("Are you sure you want to delete this customer? (y/n): "))
        {
            Console.WriteLine("Delete cancelled.");
            return;
        }

        var success = _customerService.DeleteCustomerByEmail(email);

        Console.WriteLine(success ? "Customer deleted." : "Something went wrong.");
    }

    private void ViewCustomerByNumber()
    {
        var customers = _customerService.GetAllCustomers().ToList();

        var selectedCustomer = SelectCustomerFromList("View customer (choose number)", customers);
        if (selectedCustomer == null)
            return;

        Console.Clear();
        Console.WriteLine("Customer details");
        Console.WriteLine();

        PrintCustomerDetails(selectedCustomer);
    }

    private Customer? SelectCustomerFromList(string title, List<Customer> customers)
    {
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();

        if (customers.Count == 0)
        {
            Console.WriteLine("No customers found.");
            return null;
        }

        while (true)
        {
            for (int i = 0; i < customers.Count; i++)
            {
                var customer = customers[i];
                Console.WriteLine($"[{i + 1}] {customer.FullName} - {customer.Email}");
            }

            Console.WriteLine("[0] Go back to menu");
            Console.Write("Choose number: ");

            var input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Not a valid number. Press ENTER to try again...");
                Console.ReadLine();
                Console.Clear();
                continue;
            }

            if (choice == 0)
                return null;

            if (choice < 1 || choice > customers.Count)
            {
                Console.WriteLine($"Number must be between 1 and {customers.Count}. Press ENTER to try again...");
                Console.ReadLine();
                Console.Clear();
                continue;
            }

            return customers[choice - 1];
        }
    }

    private bool ConfirmYesNo(string text)
    {
        while (true)
        {
            Console.Write(text);
            var input = (Console.ReadLine() ?? string.Empty).Trim().ToLower();

            if (input == "y")
                return true;

            if (input == "n")
                return false;

            Console.WriteLine("Please enter 'y' for yes or 'n' for no.");
        }
    }

    private void PrintCustomerDetails(Customer customer)
    {
        Console.WriteLine($"Id: {customer.Id}");
        Console.WriteLine($"Name: {customer.FullName}");
        Console.WriteLine($"Email: {customer.Email}");
        Console.WriteLine($"Phone: {customer.PhoneNumber}");
        Console.WriteLine($"Street: {customer.Address.Street}");
        Console.WriteLine($"Postal code: {customer.Address.PostalCode}");
        Console.WriteLine($"City: {customer.Address.City}");
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
}