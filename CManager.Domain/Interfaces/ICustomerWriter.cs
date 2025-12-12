using CManager.Domain.Models;
namespace CManager.Domain.Interfaces;
public interface ICustomerWriter
{
    void Add(Customer customer);
    void Update(Customer customer);
    void Delete(Guid id);
}
