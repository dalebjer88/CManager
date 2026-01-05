using CManager.Domain.Models;

namespace CManager.Application.Interfaces;

public interface ICustomerFactory
{
    Customer Create(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string street,
        string postalCode,
        string city
    );
}
