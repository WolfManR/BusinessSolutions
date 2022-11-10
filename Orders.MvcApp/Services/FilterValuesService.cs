using Microsoft.AspNetCore.Mvc;

namespace Orders.MvcApp.Services;

public class FilterValuesService
{
	private readonly IOrdersFiltersValuesClient _ordersFiltersValuesClient;

	public FilterValuesService(IOrdersFiltersValuesClient ordersFiltersValuesClient)
	{
		_ordersFiltersValuesClient = ordersFiltersValuesClient;
	}
	
	public async Task<string[]> OrdersNumbers()
	{
		try
		{
			var response = await _ordersFiltersValuesClient.GetOrdersNumbers();
			return response;
		}
		catch (Exception e)
		{
			// log
			return Array.Empty<string>();
		}
	}
	
	public async Task<string[]> Providers()
	{
		try
		{
			var response = await _ordersFiltersValuesClient.GetProviders();
			return response;
		}
		catch (Exception e)
		{
			// log
			return Array.Empty<string>();
		}
	}
	
	public async Task<string[]> OrderItemsNames()
	{
		try
		{
			var response = await _ordersFiltersValuesClient.GetOrderItemsNames();
			return response;
		}
		catch (Exception e)
		{
			// log
			return Array.Empty<string>();
		}
	}
	
	public async Task<string[]> OrderItemsUnits()
	{
		try
		{
			var response = await _ordersFiltersValuesClient.GetOrderItemsUnits();
			return response;
		}
		catch (Exception e)
		{
			// log
			return Array.Empty<string>();
		}
	}
}