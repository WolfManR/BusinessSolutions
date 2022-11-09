namespace Orders.Api.Contracts;

public sealed class OrderItemListItem
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public decimal Quantity { get; init; }
    public string Unit { get; init; } = null!;
}