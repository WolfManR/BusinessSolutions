using Orders.MvcApp.Services;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRefitClient<IOrdersApiClient>().ConfigureHttpClient((p, c) =>
{
	var configuration = p.GetRequiredService<IConfiguration>();
	c.BaseAddress = new Uri(configuration.GetConnectionString(configuration["ApiProvider"]));
});
builder.Services.AddRefitClient<IOrdersFiltersValuesClient>().ConfigureHttpClient((p, c) =>
{
	var configuration = p.GetRequiredService<IConfiguration>();
	c.BaseAddress = new Uri(configuration.GetConnectionString(configuration["ApiProvider"]) + "/filter");
});

builder.Services
	.AddScoped<OrdersService>()
	.AddScoped<FilterValuesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Errors/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Orders}/{action=Index}/{id?}");

app.Run();
