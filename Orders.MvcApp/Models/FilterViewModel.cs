namespace Orders.MvcApp.Models;

public class FilterViewModel
{
    public DateTime From { get; set; } = DateTime.Now.AddMonths(-1);
    public DateTime To { get; set; } = DateTime.Now;

    public string? OrdersNumber { get; set; }
    public string? Provider { get; set; }
    public string? OrderItemsName { get; set; }
    public string? OrderItemsUnit { get; set; }
}