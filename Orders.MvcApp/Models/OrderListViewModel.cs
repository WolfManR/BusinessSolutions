namespace Orders.MvcApp.Models;

public class OrderListViewModel
{
    public int Id { get; set; }
    public string Number { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public DateTime Date { get; set; }
}