using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace SQL_Test
{
    public class AEXSoftDbContext : DbContext
    {
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Order> CustomerOrder { get; set; }

        public AEXSoftDbContext(DbContextOptions<AEXSoftDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Retrieves a list of CustomerViewModel objects who have made purchases totaling more than the specified sumAmount since the given beginDate.
        /// </summary>
        /// <param name="beginDate">The start date for filtering purchases.</param>
        /// <param name="sumAmount">The minimum total purchase amount for filtering.</param>
        /// <returns>A list of CustomerViewModel objects that meet the filtering criteria.</returns>
        public List<CustomerViewModel> GetCustomers(DateTime beginDate, decimal sumAmount)
        {
            var customers = Customer
                .Where(c => c.Orders != null && c.Orders.Any(o => o.Date >= beginDate))
                .Select(c => new
                {
                    CustomerName = c.Name,
                    ManagerName = c.Manager != null ? c.Manager.Name : "ERROR/NoName",
                    TotalAmount = c.Orders != null ? c.Orders.Where(o => o.Date >= beginDate).Sum(o => o.Amount) : -1
                })
                .Where(c => c.TotalAmount > sumAmount)
                .ToList();

            List<CustomerViewModel> result = customers.Select(c => new CustomerViewModel
            {
                CustomerName = c.CustomerName,
                ManagerName = c.ManagerName,
                Amount = c.TotalAmount
            }).ToList();

            return result;
        }
    }

    public class Customer
    {
        public int ID { get; set; }
        public string? Name { get; set; }

        public virtual Manager? Manager { get; set; }
        public int ManagerID { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }
    }

    public class Manager
    {
        public int ID { get; set; }
        public string? Name { get; set; }
    }

    public class Order
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public virtual Customer? Customer { get; set; }
        public int CustomerID { get; set; }
    }

    public class CustomerViewModel
    {
        public string? CustomerName { get; set; }
        public string? ManagerName { get; set; }
        public decimal Amount { get; set; }
    }
}
