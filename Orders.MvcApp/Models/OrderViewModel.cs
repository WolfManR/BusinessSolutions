namespace Orders.MvcApp.Models;

public class OrderViewModel
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public int ProviderId { get; set; }
    public DateTime Date { get; set; }
    public OrderItemViewModel[] OrderItems { get; set; } = Array.Empty<OrderItemViewModel>();
}