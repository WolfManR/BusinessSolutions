namespace Orders.Api.Contracts;

public sealed class OrdersListResponse
{
    public OrderListItem[] Orders { get; set; } = null!;
}