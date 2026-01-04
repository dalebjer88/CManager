// Have had help from Emil, (his session about tests) and ChatGPT, writing this code.
using Moq;
using CManager.Application.Services;
using CManager.Application.Interfaces;
using CManager.Domain.Models;
using CManager.Domain.Interfaces;

namespace CManager.Tests.Application.Services;

public sealed class CustomerService_Tests
{
    private readonly Mock<ICustomerRepo> _customerRepositoryMock;
    private readonly Mock<IIdGenerator> _idGeneratorMock;
    private readonly ICustomerService _customerService;

    public CustomerService_Tests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepo>();
        _idGeneratorMock = new Mock<IIdGenerator>();
        _customerService = new CustomerService(_customerRepositoryMock.Object, _idGeneratorMock.Object);
    }

    [Fact]
    public void AddCustomer_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange
        _customerRepositoryMock.Setup(repo => repo.LoadAll()).Returns(new List<Customer>());

        _idGeneratorMock.Setup(gen => gen.Generate()).Returns(Guid.NewGuid());

        _customerRepositoryMock.Setup(repo => repo.Add(It.IsAny<Customer>())).Returns(true);

        // Act
        var result = _customerService.CreateCustomer(
            "Mattias",
            "Mattiasson",
            "mattias@domain.com",
            "0701234567",
            "Gatan 1",
            "12345",
            "Staden"
        );

        // Assert
        Assert.True(result);
        _customerRepositoryMock.Verify(repo => repo.Add(It.Is<Customer>(c =>
            c.FirstName == "Mattias" &&
            c.Email == "mattias@domain.com" &&
            c.Address.City == "Staden"
        )), Times.Once);
    }

    [Fact]
    public void CreateCustomer_ShouldReturnFalse_WhenEmailAlreadyExists()
    {
        // Arrange
        var existingEmail = "mattias@domain.com";
        var existingCustomers = new List<Customer>
    {
        new Customer { Email = existingEmail }
    };

        _customerRepositoryMock.Setup(repo => repo.LoadAll()).Returns(existingCustomers);

        // Act
        var result = _customerService.CreateCustomer("Mattias", "M", existingEmail, "123", "G", "123", "S");

        // Assert
        Assert.False(result);
        _customerRepositoryMock.Verify(repo => repo.Add(It.IsAny<Customer>()), Times.Never);
    }
}