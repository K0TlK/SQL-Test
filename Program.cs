using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace SQL_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=DESKTOP-HI96GF8;Database=AEXSoft;Trusted_Connection=True;Encrypt=False;";
            
            var options = new DbContextOptionsBuilder<AEXSoftDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new AEXSoftDbContext(options))
            {
                List<Customer> customers = context.Customer.ToList();

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
    }
}
