using CManager.Domain.Models;

namespace CManager.Domain.Interfaces;
public interface ICustomerListStorage
{
    List<Customer> LoadAll();
    void SaveAll(List<Customer> customers);
}
