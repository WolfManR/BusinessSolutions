namespace Orders.Api.Contracts;

public sealed class OrderDetailsResponse
{
    public string Number { get; init; } = null!;
    public DateTime Date { get; init; }
    public int ProviderId { get; init; }

    public OrderItemListItem[] OrderItems { get; init; } = null!;
}