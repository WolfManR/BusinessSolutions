using Orders.Api.Contracts;
using Orders.MvcApp.Models;

namespace Orders.MvcApp.Services;

public class OrdersService
{
	private readonly IOrdersApiClient _client;

	public OrdersService(IOrdersApiClient client)
	{
		_client = client;
	}

	public async Task<IReadOnlyCollection<OrderListViewModel>> GetOrders(OrdersListRequest request)
	{
		try
		{
			var response = await _client.GetOrders(request);

			return response.Orders.Select(x=>new OrderListViewModel()
			{
				Id = x.Id,
				Number = x.Number,
				Date = x.Date,
				Provider = x.Provider
			}).ToList();
		}
		catch (Exception e)
		{
			// log
			return Array.Empty<OrderListViewModel>();
		}
	}
	public async Task<OrderViewModel?> GetDetails(int orderId)
	{
		try
		{
			var response = await _client.GetDetails(orderId);

			return new OrderViewModel()
			{
				Id = orderId,
				Number = response.Number,
				ProviderId = response.ProviderId,
				Date = response.Date,
				OrderItems = response.OrderItems.Select(x=>new OrderItemViewModel()
				{
					Id = x.Id,
					Name = x.Name,
					Unit = x.Unit,
					Quantity = x.Quantity
				}).ToList()
			};
		}
		catch (Exception e)
		{
			// log
			return null;
		}
	}
	public async Task Delete(int orderId)
	{
		try
		{
			await _client.Delete(orderId);
			
		}
		catch (Exception e)
		{
			// log
		}
	}
	public async Task Save(OrderViewModel order)
	{
		SaveOrderRequest request = new()
		{
			Id = order.Id,
			Number = order.Number,
			ProviderId = order.ProviderId,
			Date = order.Date,
			OrderItems = order.OrderItems.Select(x=>new OrderItemListItem()
			{
				Id = x.Id,
				Name = x.Name,
				Unit = x.Unit,
				Quantity = x.Quantity
			}).ToArray()
		};
		try
		{
			await _client.Save(request);
		}
		catch (Exception e)
		{
			// log
		}
	}

	public async Task<IReadOnlyCollection<ProviderViewModel>> GetProviders()
	{
		try
		{
			var response = await _client.GetProviders();
			return response.Select(x => new ProviderViewModel() { Id = x.Id, Name = x.Name }).ToList();
		}
		catch (Exception e)
		{
			// log
			return Array.Empty<ProviderViewModel>();
		}
	}
}