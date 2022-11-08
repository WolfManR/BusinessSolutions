namespace Orders.Data;

public sealed class Order
{
    public int Id { get; init; }

    public string Number { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public int ProviderId { get; set; }
    public Provider Provider { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; init; } = new HashSet<OrderItem>();
}