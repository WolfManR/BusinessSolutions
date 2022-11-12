using Microsoft.EntityFrameworkCore;
using Orders.Api.Contracts;
using Orders.Data;

namespace Orders.Api;

public class ProvidersStorage
{
	private readonly OrdersDbContext _dbContext;

	public ProvidersStorage(OrdersDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IReadOnlyCollection<ProviderItemResponse>> GetProviders()
	{
		var providers = await _dbContext.Providers
			.Select(x => new ProviderItemResponse()
			{
				Id = x.Id,
				Name = x.Name
			})
			.AsNoTracking()
			.ToListAsync();
		return providers.AsReadOnly();
	}
}