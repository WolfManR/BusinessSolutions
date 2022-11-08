using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Orders.Api.Contracts;
using Orders.Data;
using Orders.Data.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOrdersDatabase();

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

app.MapGet("orders/all", async ([FromBody] OrdersListRequest request, [FromServices] OrdersDbContext db) =>
{
    var query = db.Orders
        .Include(x => x.OrderItems)
        .Include(x => x.Provider)
        .Where(x => x.Date >= request.From && x.Date <= request.To);

    if (request.OrderItemName is not null) query = query.Where(x => x.OrderItems.Any(c => c.Name == request.OrderItemName));
    if (request.OrderItemUnit is not null) query = query.Where(x => x.OrderItems.Any(c => c.Unit == request.OrderItemUnit));
    if (request.Provider is not null) query = query.Where(x => x.Provider.Name == request.Provider);
    if (request.OrderNumber is not null) query = query.Where(x => x.Number == request.OrderNumber);

    var orders = await query.AsNoTracking().Select(x => new OrderListItem()
    {
        Id = x.Id,
        Date = x.Date,
        Number = x.Number,
        Provider = x.Provider.Name
    }).ToListAsync();

    return Results.Ok(new OrdersListResponse()
    {
        Orders = orders.ToArray()
    });
}).Produces<OrdersListResponse>();

app.MapGet("orders/details/{orderId:int}", async ([FromRoute] int orderId, [FromServices] OrdersDbContext db) =>
{
    var order = await db.Orders.Include(x => x.Provider).Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == orderId);
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
}).Produces<OrderDetailsResponse>();

app.MapGet("orders/remove/{orderId:int}", async ([FromRoute] int orderId, [FromServices] OrdersDbContext db) =>
{
    var order = await db.Orders.FindAsync(orderId);
    if (order is null) return Results.Ok();
    db.Orders.Remove(order);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapGet("orders/save", async ([FromBody] SaveOrderRequest request, [FromServices] OrdersDbContext db) =>
{
    Order? order = null;
    if (request.Id > 0)
    {
        order = await db.Orders.Include(x => x.Provider).Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == request.Id);
    }

    if (order is null) order = new();
    var provider = await db.Providers.FindAsync(request.ProviderId);
    if (provider is null) return Results.BadRequest();

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

    db.Orders.Update(order);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.Run();
