using CManager.Domain.Models;

namespace CManager.Application.Interfaces;

public interface ICustomerService
{
    Customer CreateCustomer(
    string firstName,
    string lastName,
    string email,
    string phoneNumber,
    string street,
    string postalCode,
    string city);

    bool DeleteCustomerByEmail(string email);
    Customer? GetCustomerByEmail(string email);
    List<Customer> GetAllCustomers();
}
