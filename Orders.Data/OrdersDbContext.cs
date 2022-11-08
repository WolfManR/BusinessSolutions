using Microsoft.EntityFrameworkCore;

namespace Orders.Data;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Provider> Providers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(b =>
        {
            // Orders has OrderItems
            b.HasMany(x => x.OrderItems).WithOne(x => x.Order);
            // Order has Provider
            b.HasOne(x => x.Provider).WithMany().HasForeignKey(x=>x.ProviderId);
            // Order Number and ProviderId is unique constraint
            b.HasIndex(x => new { x.ProviderId, x.Number }).IsUnique();

            // Order Name cannot be equals to Order Number

            // Order Number, Date, ProviderId is indexed
            b.HasIndex(x => x.Number);
            b.HasIndex(x => x.Date);
        });

        modelBuilder.Entity<OrderItem>(b =>
        {
            // OrderItem Name, Unit is indexed
            b.HasIndex(x => x.Name);
            b.HasIndex(x => x.Unit);
        });

        // Provider Name is indexed
        modelBuilder.Entity<Provider>(b => b.HasIndex(x => x.Name));
    }
}