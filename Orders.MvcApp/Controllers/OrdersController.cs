using Microsoft.AspNetCore.Mvc;
using Orders.MvcApp.Models;

namespace Orders.MvcApp.Controllers;

public class OrdersController : Controller
{
    public IActionResult Index()
    {
        return Index(new());
    }

    [HttpPost]
    public IActionResult Index(FilterViewModel model)
    {
        return View(new OrdersTableViewModel()
        {
            Filter = model,
            Orders = new[]
            {
                new OrderListViewModel()
                {
                    Id = 1,
                    Number = "79f77c20-b5e7-47f7-a9e1-c1c8814ddd8e",
                    Provider = "Lorem",
                    Date = DateTime.Now.AddDays(-4)
                },
                new OrderListViewModel()
                {
                    Id = 2,
                    Number = "ed908a00-f029-4a54-a2a0-91db98ca859a",
                    Provider = "Lorem",
                    Date = DateTime.Now.AddDays(-23)
                },
                new OrderListViewModel()
                {
                    Id = 3,
                    Number = "c59a233f-0d83-4f32-a346-791aa0670f09",
                    Provider = "Ipsum",
                    Date = DateTime.Now.AddDays(-40)
                },
            }
        });
    }

    public IActionResult Details(int id)
    {
        return View(new OrderViewModel()
        {
            Id = id,
            Number = "79f77c20-b5e7-47f7-a9e1-c1c8814ddd8e",
            ProviderId = 1,
            Date = DateTime.Now.AddDays(-4),
            OrderItems = new[]
            {
                new OrderItemViewModel()
                {
                    Id = 1,
                    Name = "Chair",
                    Unit = "thing",
                    Quantity = 14
                },
                new OrderItemViewModel()
                {
                    Id = 1,
                    Name = "Wine",
                    Unit = "bottle",
                    Quantity = 2
                },
                new OrderItemViewModel()
                {
                    Id = 1,
                    Name = "Textile",
                    Unit = "metr",
                    Quantity = 10.5m
                },
            }
        });
    }

    public IActionResult Edit(int id)
    {
        if(id <= 0) return View(new OrderViewModel());

        return View(new OrderViewModel()
        {
            Id = id,
            Number = "79f77c20-b5e7-47f7-a9e1-c1c8814ddd8e",
            ProviderId = 1,
            Date = DateTime.Now.AddDays(-4),
            OrderItems = new[]
            {
                new OrderItemViewModel()
                {
                    Id = 1,
                    Name = "Chair",
                    Unit = "thing",
                    Quantity = 14
                },
                new OrderItemViewModel()
                {
                    Id = 1,
                    Name = "Wine",
                    Unit = "bottle",
                    Quantity = 2
                },
                new OrderItemViewModel()
                {
                    Id = 1,
                    Name = "Textile",
                    Unit = "metr",
                    Quantity = 10.5m
                },
            }
        });
    }

    [HttpPost]
    public IActionResult Edit(OrderViewModel model)
    {
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete()
    {
        return RedirectToAction(nameof(Index));
    }
}