using CManager.Application.Interfaces;
namespace CManager.Application.Helpers;

public class IdGenerator : IIdGenerator
{
    public Guid Generate() => Guid.NewGuid();
}