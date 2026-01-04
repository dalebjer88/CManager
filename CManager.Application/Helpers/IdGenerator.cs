using CManager.Application.Interfaces;

namespace CManager.Application.Helpers;

public sealed class IdGenerator : IIdGenerator
{
    public Guid Generate() => Guid.NewGuid();
}