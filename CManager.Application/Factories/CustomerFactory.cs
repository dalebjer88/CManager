using CManager.Application.Interfaces;
using CManager.Domain.Models;

namespace CManager.Application.Factories;

public sealed class CustomerFactory : ICustomerFactory
{
    public Customer Create(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string street,
        string postalCode,
        string city
    )
    {
        return new Customer
        {
            Id = id,
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
    }
}
