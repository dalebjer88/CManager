using CManager.Domain.Models;

namespace CManager.Domain.Interfaces;
public interface ICustomerWriter
{
    bool Add(Customer customer);
    bool Update(Customer customer);
    bool Delete(Guid id);
}
