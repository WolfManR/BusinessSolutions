using Microsoft.AspNetCore.Mvc;

using Orders.Api.Contracts;
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

app.MapGet("orders/all", ([FromBody] OrdersListRequest? request) =>
{
    return Results.Ok(new OrdersListResponse());
}).Produces<OrdersListResponse>();

app.MapGet("orders/details/{orderId:int}", ([FromRoute] int orderId) =>
{
    return Results.Ok(new OrderDetailsResponse());
}).Produces<OrderDetailsResponse>();

app.MapGet("orders/remove/{orderId:int}", ([FromRoute] int orderId) => { return Results.Ok(); });

app.MapGet("orders/save", () => { return Results.Ok(); });

app.Run();
