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
        // string properties with type nvarchar(max) or varchar(max) cannot be indexed
        // possible solution to make hash properties, that will store checksum values and
        // its will be used in indexes with type nvarchar(450) as required by ef

        modelBuilder.Entity<Order>(b =>
        {
            // Orders has OrderItems
            b.HasMany(x => x.OrderItems).WithOne(x => x.Order);
            // Order has Provider
            b.HasOne(x => x.Provider).WithMany().HasForeignKey(x=>x.ProviderId);

            // Date has precision in milliseconds with 7
            b.Property(x => x.Date).HasPrecision(7);

            // Order Name length max as possible
            b.Property(x => x.Number).HasMaxLength(int.MaxValue);

            // Order Number and ProviderId is unique constraint
            //b.HasIndex(x => new { x.ProviderId, x.Number }).IsUnique();

            // Order Number, Date, ProviderId is indexed
            //b.HasIndex(x => x.Number);
            b.HasIndex(x => x.Date);
        });

        modelBuilder.Entity<OrderItem>(b =>
        {
            // Quantity has precision 18, 3
            b.Property(x => x.Quantity).HasPrecision(18, 3);

            // OrderItem Name and Unit length max as possible
            b.Property(x => x.Name).HasMaxLength(int.MaxValue);
            b.Property(x => x.Unit).HasMaxLength(int.MaxValue);

            // OrderItem Name cannot be equals to Order Number
            // may be handled by https://www.nuget.org/packages/EFCore.CheckConstraints nuget package extension

            // Name, Unit is indexed
            //b.HasIndex(x => x.Name);
            //b.HasIndex(x => x.Unit);
        });

        modelBuilder.Entity<Provider>(b =>
        {
            // Provider Name length max as possible
            b.Property(x => x.Name).HasMaxLength(int.MaxValue);

            // Provider Name is indexed
            //b.HasIndex(x => x.Name);
        });
    }
}