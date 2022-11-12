using Refit;

namespace Orders.MvcApp.Services;

public interface IOrdersFiltersValuesClient
{
	[Get("/order-numbers")]
	Task<string[]> GetOrdersNumbers();

	[Get("/providers")]
	Task<string[]> GetProviders();

	[Get("/order-items-names")]
	Task<string[]> GetOrderItemsNames();

	[Get("/order-items-units")]
	Task<string[]> GetOrderItemsUnits();
}