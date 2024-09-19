
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Models;

public class OrderDbContext : IdentityDbContext<Microsoft.AspNetCore.Identity.IdentityUser>
{


    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<Order>()
       .HasOne(o => o.User)
       .WithMany() 
       .HasForeignKey(o => o.UserId)
       .OnDelete(DeleteBehavior.Restrict); 

      

        // Seed data for Products
        modelBuilder.Entity<Product>().HasData(
            new Product { ProductId = 1, ProductName = "Product A" },
            new Product { ProductId = 2, ProductName = "Product B" },
            new Product { ProductId = 3, ProductName = "Product C" }
        );
        modelBuilder.Entity<Order>().HasData(
            new Order { OrderId = 1, UserId = "1", ProductId = 1 }, 
            new Order { OrderId = 2, UserId = "1", ProductId = 2 }  
        );
    }

}
