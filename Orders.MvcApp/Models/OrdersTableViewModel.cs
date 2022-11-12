namespace Orders.MvcApp.Models;

public class OrdersTableViewModel
{
    public FilterViewModel Filter { get; set; } = new();
    public OrderListViewModel[] Orders { get; set; } = null!;
}