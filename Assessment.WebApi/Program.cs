using Assessment.Business.Services.OrderService;
using Assessment.Domain.ChannelEngineClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IChannelEngineClient, ChannelEngineClient>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IChannelEngineClient, ChannelEngineClient>();
builder.Services.AddScoped<HttpClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/ChannelEngine/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ChannelEngine}/{action=Index}/{id?}");

app.Run();
