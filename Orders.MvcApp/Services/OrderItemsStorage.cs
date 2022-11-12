using System.Text.Json;
using Orders.MvcApp.Models;

namespace Orders.MvcApp.Services;

public class OrderItemsStorage
{
	private readonly IHttpContextAccessor _contextAccessor;

	private readonly string _sessionId;

	public OrderItemsStorage(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;

		_sessionId = "sessionOrders";
	}

	public List<OrderItemViewModel> Items
	{
		get
		{
			var context = _contextAccessor.HttpContext;
			var cookies = context.Response.Cookies;
			var storageCookie = context.Request.Cookies[_sessionId];
			if (storageCookie is null)
			{
				List<OrderItemViewModel> items = new();
				cookies.Append(_sessionId, JsonSerializer.Serialize(items));
				return items;
			}

			ReplaceCookie(cookies, storageCookie);
			return JsonSerializer.Deserialize<List<OrderItemViewModel>>(storageCookie);
		}
		set => ReplaceCookie(_contextAccessor.HttpContext.Response.Cookies, JsonSerializer.Serialize(value));
	}

	public void AddItem(OrderItemViewModel item)
	{
		var items = Items;
		var existed = items.FirstOrDefault(x => ItemComparer(x, item));

		if (existed is null) items.Add(item);
		else existed.Quantity += item.Quantity;

		Items = items;
	}

	public void RemoveItem(OrderItemViewModel item)
	{
		var items = Items;
		var existed = items.FirstOrDefault(x => ItemComparer(x, item));
		if (existed is null) return;
		items.Remove(existed);
		Items = items;
	}

	public void Clear()
	{
		Items = new();
	}

	private bool ItemComparer(OrderItemViewModel source, OrderItemViewModel searched) => 
		 source.Name == searched.Name && source.Unit == searched.Unit;

	private void ReplaceCookie(IResponseCookies cookies, string cookie)
	{
		cookies.Delete(_sessionId);
		cookies.Append(_sessionId, cookie);
	}
}