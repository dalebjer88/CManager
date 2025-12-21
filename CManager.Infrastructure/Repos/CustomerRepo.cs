using CManager.Domain.Interfaces;
using CManager.Domain.Models;
namespace CManager.Infrastructure.Repos;

public class CustomerRepo : ICustomerRepo
{
    private readonly string _filePath;
    private readonly IJsonFormatter _formatter;

    public CustomerRepo(string filePath, IJsonFormatter formatter)
    {
        _filePath = filePath;
        _formatter = formatter;

    }

    public List<Customer> LoadAll()
    {
        if (!File.Exists(_filePath))
            return new List<Customer>();

        var json = File.ReadAllText(_filePath);
        return _formatter.Deserialize<List<Customer>>(json) ?? new List<Customer>();
    }

    public void SaveAll(List<Customer> customers)
    {
        var json = _formatter.Serialize(customers);
        File.WriteAllText(_filePath, json);
    }

    public Customer? GetById(Guid id)
    {
        var customers = LoadAll();
        return customers.FirstOrDefault(c => c.Id == id);
    }

    public bool Add(Customer customer)
    {
        var customers = LoadAll();

        if (customers.Any(c => c.Id == customer.Id))
            return false;

        customers.Add(customer);
        SaveAll(customers);
        return true;
    }

    public bool Update(Customer customer)
    {
        var customers = LoadAll();
        var index = customers.FindIndex(c => c.Id == customer.Id);

        if (index < 0)
            return false;

        customers[index] = customer;
        SaveAll(customers);
        return true;
    }

    public bool Delete(Guid id)
    {
        var customers = LoadAll();
        var customerToRemove = customers.FirstOrDefault(c => c.Id == id);

        if (customerToRemove == null)
            return false;

        customers.Remove(customerToRemove);
        SaveAll(customers);
        return true;
    }
}
