using CManager.Application.Interfaces;
using CManager.Domain.Interfaces;
using CManager.Domain.Models;

namespace CManager.Application.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerRepo _repo;
    private readonly IIdGenerator _idGenerator;
    private readonly ICustomerFactory _customerFactory;

    public CustomerService(ICustomerRepo repo, IIdGenerator idGenerator, ICustomerFactory customerFactory)
    {
        _repo = repo;
        _idGenerator = idGenerator;
        _customerFactory = customerFactory;
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        return _repo.LoadAll();
    }

    public Customer? GetCustomerByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var customers = _repo.LoadAll();
        var normalizedEmail = email.Trim();
        return customers.FirstOrDefault(
            c => c.Email.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase));

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

        var normalizedEmail = email.Trim();
        var customers = _repo.LoadAll();

        if (customers.Any(c => c.Email.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase)))
            return false;

        var id = _idGenerator.Generate();

        var customer = _customerFactory.Create(
            id,
            firstName.Trim(),
            lastName.Trim(),
            normalizedEmail,
            phoneNumber.Trim(),
            street.Trim(),
            postalCode.Trim(),
            city.Trim()
        );

        return _repo.Add(customer);
    }
    public bool UpdateCustomer(Customer customer)
    {
        return _repo.Update(customer);
    }

    public bool DeleteCustomerByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var customers = _repo.LoadAll();
        var normalizedEmail = email.Trim();
        var customer = customers.FirstOrDefault(
            c => c.Email.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase));


        if (customer == null)
            return false;

        return _repo.Delete(customer.Id);
    }

    public Customer? GetCustomerById(Guid id)
    {
        return _repo.GetById(id);
    }

    public bool DeleteCustomerById(Guid id)
    {
        return _repo.Delete(id);
    }
}
