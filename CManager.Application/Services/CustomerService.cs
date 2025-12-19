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

    public bool CreateCustomer(
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string street,
        string postalCode,
        string city)
    {
        if (string.IsNullOrWhiteSpace(firstName)) return false;
        if (string.IsNullOrWhiteSpace(lastName)) return false;
        if (string.IsNullOrWhiteSpace(email)) return false;

        if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
        if (string.IsNullOrWhiteSpace(street)) return false;
        if (string.IsNullOrWhiteSpace(postalCode)) return false;
        if (string.IsNullOrWhiteSpace(city)) return false;

        if (_customers.Any(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            return false;

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = email.Trim(),
            PhoneNumber = phoneNumber.Trim(),
            Address = new Address
            {
                Street = street.Trim(),
                PostalCode = postalCode.Trim(),
                City = city.Trim()
            }
        };

        _customers.Add(customer);

        try
        {
            _storage.SaveAll(_customers);
            return true;
        }
        catch
        {
            _customers.Remove(customer);
            return false;
        }
    }

    public bool DeleteCustomerByEmail(string email)
    {
        var customer = GetCustomerByEmail(email);
        if (customer == null)
            return false;

        _customers.Remove(customer);

        try
        {
            _storage.SaveAll(_customers);
            return true;
        }
        catch
        {
            _customers.Add(customer);
            return false;
        }
    }

}
