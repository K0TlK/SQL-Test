using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SQL_Test;

IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

string? connectionString = configuration.GetConnectionString("DefaultConnection");

if (connectionString != null)
{
    var options = new DbContextOptionsBuilder<AEXSoftDbContext>()
    .UseSqlServer(connectionString)
    .Options;

    using (var context = new AEXSoftDbContext(options))
    {
        List<Customer> customers = context.Customers.ToList();

        foreach (var customer in customers)
        {
            Console.WriteLine($"ID: {customer.ID}, Name: {customer.Name}");
        }
    }

    Console.WriteLine();

    using (var context = new AEXSoftDbContext(options))
    {
        DateTime beginDate = new DateTime(2023, 01, 01);
        decimal sumAmount = 1000;

        var customers = context.GetCustomers(beginDate, sumAmount);

        foreach (var customer in customers)
        {
            Console.WriteLine($"Customer: {customer.CustomerName}, Manager: {customer.ManagerName}, Amount: {customer.Amount}");
        }
    }

    Console.WriteLine();
}
