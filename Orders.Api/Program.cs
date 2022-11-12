using Microsoft.AspNetCore.Mvc;
using Orders.Api;
using Orders.Api.Contracts;
using Orders.Data;
using Orders.Data.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
	.AddOrdersDatabase()
	.AddScoped<OrdersStorage>()
	.AddScoped<ProvidersStorage>()
	.AddScoped<OrdersFilterValuesStorage>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    using var context = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    if (!context.Providers.Any())
    {
        context.Providers.AddRange(new[]
        {
            new Provider(){Name = "Lorem"},
            new Provider(){Name = "Ipsum"},
            new Provider(){Name = "Koren"},
            new Provider(){Name = "Laramusha"},
        });
        context.SaveChanges();
    }
}

app.MapPost("orders/all", async ([FromBody] OrdersListRequest request, [FromServices] OrdersStorage storage) =>
{
	var orders = await storage.GetOrders(request);

    return Results.Ok(new OrdersListResponse()
    {
        Orders = orders.ToArray()
    });
}).Produces<OrdersListResponse>().WithTags("Orders");

app.MapGet("orders/details/{orderId:int}", async ([FromRoute] int orderId, [FromServices] OrdersStorage storage) =>
{
	var order = await storage.GetOrder(orderId);

    if (order is null) return Results.NotFound();

    return Results.Ok(new OrderDetailsResponse()
    {
        Number = order.Number,
        Date = order.Date,
        ProviderId = order.Provider.Id,
        OrderItems = order.OrderItems.Select(x => new OrderItemListItem()
        {
            Id = x.Id,
            Name = x.Name,
            Unit = x.Unit,
            Quantity = x.Quantity
        }).ToArray()
    });
}).Produces<OrderDetailsResponse>().ProducesProblem(404).WithTags("Orders");

app.MapDelete("orders/remove/{orderId:int}", async ([FromRoute] int orderId, [FromServices] OrdersStorage storage) =>
{
	await storage.RemoveOrder(orderId);
    return Results.Ok();
}).Produces(200).WithTags("Orders");

app.MapPost("orders/save", async ([FromBody] SaveOrderRequest request, [FromServices] OrdersStorage storage) =>
{
	var isAnyOrderItemHasSameNameAsOrderNumber = request.OrderItems.Any(x => x.Name == request.Number);
	if (isAnyOrderItemHasSameNameAsOrderNumber)
	{
		return Results.BadRequest("Order items cannot be named as order number");
	}

	if (request.Id == default && await storage.IsOrderWithProviderAndNumberExist(request.ProviderId, request.Number))
	{
		return Results.BadRequest("Order for same provider with same number already exist");
	}

	var result = await storage.AddUpdateOrder(request);
	if (!result) return Results.BadRequest();

    return Results.Ok();
}).ProducesProblem(400).Produces(200).WithTags("Orders");

app.MapGet("filter/order-numbers", async ([FromServices] OrdersFilterValuesStorage storage) =>
{
	var data = await storage.OrdersNumbers();
    return Results.Ok(data);
}).Produces<string[]>().WithTags("Filters");

app.MapGet("filter/providers", async ([FromServices] OrdersFilterValuesStorage storage) =>
{
    var data = await storage.Providers();
    return Results.Ok(data);
}).Produces<string[]>().WithTags("Filters");

app.MapGet("filter/order-items-names", async ([FromServices] OrdersFilterValuesStorage storage) =>
{
	var data = await storage.OrderItemsNames();
    return Results.Ok(data);
}).Produces<string[]>().WithTags("Filters");

app.MapGet("filter/order-items-units", async ([FromServices] OrdersFilterValuesStorage storage) =>
{
	var data = await storage.OrderItemsUnits();
    return Results.Ok(data);
}).Produces<string[]>().WithTags("Filters");

app.MapGet("providers", async ([FromServices] ProvidersStorage storage) =>
{
	var data = await storage.GetProviders();
    return Results.Ok(data);
}).Produces<ProviderItemResponse[]>().WithTags("Providers");

app.Run();
