using Microsoft.AspNetCore.Mvc;
using Orders.MvcApp.Models;
using Orders.MvcApp.Services;

namespace Orders.MvcApp.Controllers;

public class OrdersController : Controller
{
	private readonly OrdersService _ordersService;

	public OrdersController(OrdersService ordersService)
	{
		_ordersService = ordersService;
	}

    public async Task<IActionResult> Index()
    {
	    var orders = await _ordersService.GetOrders(DateTime.Now.AddMonths(-1), DateTime.Now);
        return View(new OrdersTableViewModel { Orders = orders.ToArray() });
    }

    [HttpPost]
    public async Task<IActionResult> Index(FilterViewModel model)
    {
	    var orders = await _ordersService.GetOrders(model.From, model.To);
	    return View(new OrdersTableViewModel { Filter = model, Orders = orders.ToArray() });
    }

    public async Task<IActionResult> Details(int id)
    {
	    var details = await _ordersService.GetDetails(id);
	    if (details is null)
	    {
		    return NotFound();
	    }

        return View(details);
    }

    public async Task<IActionResult> Edit(int id)
    {
        if(id <= 0) return View(new OrderViewModel());

        var details = await _ordersService.GetDetails(id);
        if (details is null)
        {
	        return NotFound();
        }
		return View(details);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(OrderViewModel model)
    {
	    await _ordersService.Save(model);
        // TODO: failure
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
	    await _ordersService.Delete(id);
	    // TODO: failure
		return RedirectToAction(nameof(Index));
    }
}