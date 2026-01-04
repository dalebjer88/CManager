using CManager.Domain.Models;

namespace CManager.Application.Interfaces;

public interface ICustomerService
{
    bool CreateCustomer(string firstName, string lastName, string email, string phoneNumber, string street, string postalCode, string city);

    IEnumerable<Customer> GetAllCustomers(); 
    Customer? GetCustomerByEmail(string email);
    Customer? GetCustomerById(Guid id);

    bool UpdateCustomer(Customer customer);

    bool DeleteCustomerByEmail(string email);
    bool DeleteCustomerById(Guid id);

}