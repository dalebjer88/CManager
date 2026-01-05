// Have had help from Emil, (his session about tests) and ChatGPT, writing this code.
using Moq;
using CManager.Application.Interfaces;
using CManager.Application.Services;
using CManager.Domain.Models;
using CManager.Domain.Interfaces;

namespace CManager.Tests.Application.Services;

public sealed class CustomerService_Tests
{
    private readonly Mock<ICustomerRepo> _customerRepositoryMock;
    private readonly Mock<IIdGenerator> _idGeneratorMock;
    private readonly Mock<ICustomerFactory> _customerFactoryMock;
    private readonly ICustomerService _customerService;

    public CustomerService_Tests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepo>();
        _idGeneratorMock = new Mock<IIdGenerator>();
        _customerFactoryMock = new Mock<ICustomerFactory>();

        _customerService = new CustomerService(
            _customerRepositoryMock.Object,
            _idGeneratorMock.Object,
            _customerFactoryMock.Object
        );
    }

    [Fact]
    public void AddCustomer_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange
        _customerRepositoryMock
            .Setup(repo => repo.LoadAll())
            .Returns(new List<Customer>());

        var expectedId = Guid.NewGuid();
        _idGeneratorMock
            .Setup(gen => gen.Generate())
            .Returns(expectedId);

        var createdCustomer = new Customer
        {
            Id = expectedId,
            FirstName = "Mattias",
            LastName = "Mattiasson",
            Email = "mattias@domain.com",
            PhoneNumber = "0701234567",
            Address = new Address
            {
                Street = "Gatan 1",
                PostalCode = "12345",
                City = "Staden"
            }
        };

        _customerFactoryMock
            .Setup(factory => factory.Create(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(createdCustomer);

        _customerRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<Customer>()))
            .Returns(true);

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

        _customerFactoryMock.Verify(factory => factory.Create(
            expectedId,
            "Mattias",
            "Mattiasson",
            "mattias@domain.com",
            "0701234567",
            "Gatan 1",
            "12345",
            "Staden"
        ), Times.Once);

        _customerRepositoryMock.Verify(repo => repo.Add(createdCustomer), Times.Once);
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

        _customerRepositoryMock
            .Setup(repo => repo.LoadAll())
            .Returns(existingCustomers);

        // Act
        var result = _customerService.CreateCustomer("Mattias", "M", existingEmail, "123", "G", "123", "S");

        // Assert
        Assert.False(result);

        _customerFactoryMock.Verify(factory => factory.Create(
            It.IsAny<Guid>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
        ), Times.Never);

        _customerRepositoryMock.Verify(repo => repo.Add(It.IsAny<Customer>()), Times.Never);
    }
}
