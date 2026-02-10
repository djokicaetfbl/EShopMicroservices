
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// PRVO DOCKER SET AS STARTUP ROJECT ( NE GASIM APP) ONDA Shopping.Web STAVIM KAO SET AS STARTUP PROJECT I SVE RADI

builder.Services.AddRefitClient<ICatalogService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!)); // base adress is YarpApiGateway address


builder.Services.AddRefitClient<IBasketService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!)); // registruj servis da ide preko reffit-a

builder.Services.AddRefitClient<IOrderingService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
