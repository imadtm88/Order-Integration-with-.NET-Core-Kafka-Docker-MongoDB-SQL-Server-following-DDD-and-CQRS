using Domain.Entity;
using Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class OrderIntegratorDbContext : DbContext
    {
        public OrderIntegratorDbContext(DbContextOptions<OrderIntegratorDbContext> options) : base(options)
        {
        }
        public DbSet<OrderModel> OrderModel { get; set; }
    }
}