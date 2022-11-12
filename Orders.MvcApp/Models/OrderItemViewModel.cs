namespace Orders.MvcApp.Models;

public class OrderItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Unit { get; set; } = null!;
    public decimal Quantity { get; set; }
}