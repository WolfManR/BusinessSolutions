using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Orders.Api.Contracts;
using Orders.MvcApp.Models;
using Orders.MvcApp.Services;

namespace Orders.MvcApp.Controllers;

public class OrdersController : Controller
{
	private readonly OrdersService _ordersService;
	private readonly OrderItemsStorage _orderItemsStorage;

	public OrdersController(OrdersService ordersService, OrderItemsStorage orderItemsStorage)
	{
		_ordersService = ordersService;
		_orderItemsStorage = orderItemsStorage;
	}

    public async Task<IActionResult> Index(OrdersTableViewModel? model, [FromServices] FilterValuesService filterValuesService)
    {
	    var filter = model?.Filter;
	    OrdersListRequest request = new()
	    {
			From = filter?.From ?? DateTime.Now.AddMonths(-1),
			To = filter?.To ?? DateTime.Now,
			OrderNumber = filter?.OrdersNumber,
			Provider = filter?.Provider,
			OrderItemName = filter?.OrderItemsName,
			OrderItemUnit = filter?.OrderItemsUnit
	    };
	    var orders = await _ordersService.GetOrders(request);

	    var ordersNumbers = filterValuesService.OrdersNumbers();
	    var providers = filterValuesService.Providers();
	    var orderItemsNames = filterValuesService.OrderItemsNames();
	    var orderItemsUnits = filterValuesService.OrderItemsUnits();
		
	    await Task.WhenAll(ordersNumbers, providers, orderItemsNames, orderItemsUnits);
	    OrdersTableViewModel vm = new() { Filter = model?.Filter ?? new(), Orders = orders.ToArray() };

		ViewBag.OrdersNumbers = new SelectList(await ordersNumbers, vm.Filter.OrdersNumber);
	    ViewBag.Providers = new SelectList(await providers, vm.Filter.Provider);
	    ViewBag.OrderItemsNames = new SelectList(await orderItemsNames, vm.Filter.OrderItemsName);
	    ViewBag.OrderItemsUnits = new SelectList(await orderItemsUnits, vm.Filter.OrderItemsUnit);

        return View(vm);
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
	    OrderViewModel? model = null;

	    if (id > 0)
	    {
		    model = await _ordersService.GetDetails(id);
		    if (model is null)
		    {
			    return NotFound();
		    }
	    }

	    ViewBag.Providers = new SelectList(await _ordersService.GetProviders(), nameof(ProviderViewModel.Id), nameof(ProviderViewModel.Name));

		if (model is not null)
	    {
		    _orderItemsStorage.Items = model.OrderItems.ToList();
	    }

	    return View(model ?? new());
    }

    [HttpPost]
	[ActionName(nameof(Edit))]
    public async Task<IActionResult> SaveOrderChanges(OrderViewModel model)
    {
	    model.OrderItems = _orderItemsStorage.Items;
	    if (!await _ordersService.Save(model))
	    {
		    ViewBag.Providers = new SelectList(await _ordersService.GetProviders(), nameof(ProviderViewModel.Id), nameof(ProviderViewModel.Name));
			return View(nameof(Edit), model);
	    }  

	    _orderItemsStorage.Clear();
        
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
	    if (!await _ordersService.Delete(id))
	    {
		    return Problem();
	    }

		return RedirectToAction(nameof(Index));
    }
	
    public IActionResult AddOrderItemToStorage(OrderItemViewModel model)
    {
		_orderItemsStorage.AddItem(model);
		return Json(model);
    }
	
    public IActionResult RemovedOrderItemFromStorage(OrderItemViewModel model)
    {
		_orderItemsStorage.RemoveItem(model);
		return Ok();
    }
}