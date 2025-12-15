using CManager.Application.Interfaces;
using CManager.Domain.Interfaces;
using CManager.Domain.Models;
namespace CManager.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerListStorage _storage;
    private readonly List<Customer> _customers;

    public CustomerService(ICustomerListStorage storage)
    {
        _storage = storage;
        _customers = _storage.LoadAll();
    }

    public List<Customer> GetAllCustomers()
    {
        return _customers;
    }

    public Customer? GetCustomerByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        return _customers.FirstOrDefault(
            c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public Customer CreateCustomer(
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string street,
        string postalCode,
        string city)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            Address = new Address
            {
                Street = street,
                PostalCode = postalCode,
                City = city
            }
        };

        _customers.Add(customer);
        _storage.SaveAll(_customers);

        return customer;
    }

    public bool DeleteCustomerByEmail(string email)
    {
        var customer = GetCustomerByEmail(email);
        if (customer == null)
        {
            return false;
        }

        _customers.Remove(customer);
        _storage.SaveAll(_customers);

        return true;
    }
}
