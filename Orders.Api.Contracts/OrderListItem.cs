namespace Orders.Api.Contracts;

public sealed class OrderListItem
{
    public int Id { get; init; }
    public string Number { get; init; } = null!;
    public DateTime Date { get; init; }
    public string Provider { get; init; } = null!;
}