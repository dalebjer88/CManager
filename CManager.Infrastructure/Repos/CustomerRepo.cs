using System.Text.Json;
using CManager.Domain.Interfaces;
using CManager.Domain.Models;
namespace CManager.Infrastructure.Repositories;

public class CustomerRepo : ICustomerRepo
{
    private readonly string _filePath;

    public CustomerRepo(string filePath)
    {
        _filePath = filePath;
    }

    public List<Customer> LoadAll()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Customer>();
        }

        var json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Customer>();
        }

        var customers = JsonSerializer.Deserialize<List<Customer>>(json);

        return customers ?? new List<Customer>();
    }

    public void SaveAll(List<Customer> customers)
    {
        var json = JsonSerializer.Serialize(customers, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_filePath, json);
    }

    public Customer? GetById(Guid id)
    {
        var customers = LoadAll();
        return customers.FirstOrDefault(c => c.Id == id);
    }

    public void Add(Customer customer)
    {
        var customers = LoadAll();
        customers.Add(customer);
        SaveAll(customers);
    }

    public void Update(Customer customer)
    {
        var customers = LoadAll();
        var index = customers.FindIndex(c => c.Id == customer.Id);

        if (index >= 0)
        {
            customers[index] = customer;
            SaveAll(customers);
        }
    }

    public void Delete(Guid id)
    {
        var customers = LoadAll();
        var customerToRemove = customers.FirstOrDefault(c => c.Id == id);

        if (customerToRemove != null)
        {
            customers.Remove(customerToRemove);
            SaveAll(customers);
        }
    }
}
