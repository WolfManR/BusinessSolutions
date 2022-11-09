using Microsoft.AspNetCore.Mvc;
using Orders.MvcApp.Models;

namespace Orders.MvcApp.Controllers;

public class OrdersController : Controller
{
    public IActionResult Index()
    {
        return View(new OrdersTableViewModel());
    }

    public IActionResult Details()
    {
        return View(new OrderViewModel());
    }

    public IActionResult Edit()
    {
        return View(new OrderViewModel());
    }
}