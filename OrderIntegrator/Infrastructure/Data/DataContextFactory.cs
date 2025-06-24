using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data
{
    public class DataContextFactory : IDesignTimeDbContextFactory<OrderIntegratorDbContext>
    {
        public OrderIntegratorDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderIntegratorDbContext>();

            var connectionString = "server=localhost\\sqlexpress;database=OrdersDB;trusted_connection=true;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);

            return new OrderIntegratorDbContext(optionsBuilder.Options);
        }
    }
}
