namespace Orders.Data;

public sealed class OrderItem
{
    public int Id { get; init; }

    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;

    public Order Order { get; init; } = null!;
}