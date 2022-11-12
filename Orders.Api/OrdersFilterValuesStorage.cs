using Microsoft.EntityFrameworkCore;
using Orders.Data;

namespace Orders.Api;

public class OrdersFilterValuesStorage
{
	private readonly OrdersDbContext _dbContext;

	public OrdersFilterValuesStorage(OrdersDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IReadOnlyCollection<string>> OrdersNumbers()
	{
		var filterValues = await _dbContext.Orders.Select(x => x.Number).Distinct().ToListAsync();
		return filterValues.AsReadOnly();
	}

	public async Task<IReadOnlyCollection<string>> Providers()
	{
		var filterValues =await _dbContext.Providers.Select(x => x.Name).Distinct().ToListAsync();
		return filterValues.AsReadOnly();
	}

	public async Task<IReadOnlyCollection<string>> OrderItemsNames()
	{
		var filterValues = await _dbContext.OrderItems.Select(x => x.Name).Distinct().ToListAsync();
		return filterValues.AsReadOnly();
	}

	public async Task<IReadOnlyCollection<string>> OrderItemsUnits()
	{
		var filterValues = await _dbContext.OrderItems.Select(x => x.Unit).Distinct().ToListAsync();
		return filterValues.AsReadOnly();
	}
}