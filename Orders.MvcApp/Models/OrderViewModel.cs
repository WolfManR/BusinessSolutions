namespace Orders.MvcApp.Models;

public class OrderViewModel
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public int ProviderId { get; set; }
    public DateTime Date { get; set; }
    public List<OrderItemViewModel> OrderItems { get; set; } = new();

    public OrderItemViewModel EditForm { get; set; } = new();
}