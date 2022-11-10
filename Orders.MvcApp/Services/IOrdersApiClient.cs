using Orders.Api.Contracts;
using Refit;

namespace Orders.MvcApp.Services;

public interface IOrdersApiClient
{
	[Post("/all")]
	Task<OrdersListResponse> GetOrders([Body] OrdersListRequest request);

	[Get("/details/{orderId:int}")]
	Task<OrderDetailsResponse> GetDetails(int orderId);

	[Delete("/remove/{orderId:int}")]
	Task Delete(int orderId);

	[Post("/save")]
	Task Save([Body] SaveOrderRequest request);
}