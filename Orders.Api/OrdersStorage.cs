using Microsoft.EntityFrameworkCore;
using Orders.Api.Contracts;
using Orders.Data;

namespace Orders.Api;

public class OrdersStorage
{
	private readonly OrdersDbContext _dbContext;

	public OrdersStorage(OrdersDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IReadOnlyCollection<OrderListItem>> GetOrders(OrdersListRequest request)
	{
		var query = _dbContext.Orders.Where(x => x.Date >= request.From && x.Date <= request.To);
		if (request.OrderNumber is not null) query = query.Where(x => x.Number == request.OrderNumber);

		if (request.Provider is not null)
		{
			query = query
			.Include(x => x.Provider)
				.Where(x => x.Provider.Name == request.Provider);
		}

		if (request.OrderItemName is not null || request.OrderItemUnit is not null)
		{
			query = query.Include(x => x.OrderItems);
			if (request.OrderItemName is not null) query = query.Where(x => x.OrderItems.Any(c => c.Name == request.OrderItemName));
			if (request.OrderItemUnit is not null) query = query.Where(x => x.OrderItems.Any(c => c.Unit == request.OrderItemUnit));
		}

		var orders = await query.AsNoTracking()
			.Select(x => new OrderListItem()
			{
				Id = x.Id,
				Date = x.Date,
				Number = x.Number,
				Provider = x.Provider.Name
			})
			.ToListAsync();

		return orders.AsReadOnly();
	}

	public async Task<Order?> GetOrder(int id)
	{
		var order = await _dbContext.Orders
			.Include(x => x.Provider)
			.Include(x => x.OrderItems)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == id);
		return order;
	}

	public async Task<bool> RemoveOrder(int id)
	{
		var order = await _dbContext.Orders.FindAsync(id);
		if (order is null) return true;

		_dbContext.Orders.Remove(order);
		await _dbContext.SaveChangesAsync();

		return true;
	}

	public async Task<bool> AddUpdateOrder(SaveOrderRequest request)
	{
		Order? order = null;
		if (request.Id > 0)
		{
			order = await _dbContext.Orders.Include(x => x.Provider).Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == request.Id);
		}

		order ??= new();
		var provider = await _dbContext.Providers.FindAsync(request.ProviderId);
		if (provider is null) return false;

		order.Number = request.Number;
		order.Date = request.Date;
		order.Provider = provider;

		order.OrderItems.Clear();

		foreach (var item in request.OrderItems)
		{
			order.OrderItems.Add(new OrderItem()
			{
				Id = item.Id,
				Unit = item.Unit,
				Name = item.Name,
				Quantity = item.Quantity,
				Order = order
			});
		}

		_dbContext.Orders.Update(order);
		await _dbContext.SaveChangesAsync();

		return true;
	}

	public async Task<bool> IsOrderWithProviderAndNumberExist(int providerId, string orderNumber)
	{
		var result = await _dbContext.Orders
			.Include(x => x.Provider)
			.AnyAsync(x => x.Provider.Id == providerId && x.Number == orderNumber);
		return result;
	}
}