using CManager.Domain.Models;

namespace CManager.Domain.Interfaces;
public interface ICustomerReader
{
    Customer? GetById(Guid id);
}
