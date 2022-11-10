using Orders.Api.Contracts;
using Refit;

namespace Orders.MvcApp.Services;

public interface IOrdersApiClient
{
	[Post("/orders/all")]
	Task<OrdersListResponse> GetOrders([Body] OrdersListRequest request);

	[Get("/orders/details/{orderId}")]
	Task<OrderDetailsResponse> GetDetails(int orderId);

	[Delete("/orders/remove/{orderId}")]
	Task Delete(int orderId);

	[Post("/orders/save")]
	Task Save([Body] SaveOrderRequest request);

	[Get("/providers")]
	Task<IEnumerable<ProviderItemResponse>> GetProviders();
}