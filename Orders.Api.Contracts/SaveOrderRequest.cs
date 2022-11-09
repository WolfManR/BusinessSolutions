namespace Orders.Api.Contracts;

public sealed class SaveOrderRequest
{
    public int Id { get; init; }
    public string Number { get; init; } = null!;
    public DateTime Date { get; init; }
    public int ProviderId { get; init; }
    public OrderItemListItem[] OrderItems { get; init; } = null!;
}