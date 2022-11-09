namespace Orders.Api.Contracts;

public sealed class OrdersListRequest
{
    public DateTime From { get; init; } = DateTime.Now.AddMonths(-1);
    public DateTime To { get; init; } = DateTime.Now;

    public string? OrderNumber { get; set; }
    public string? Provider { get; set; }
    public string? OrderItemName { get; set; }
    public string? OrderItemUnit { get; set; }
}