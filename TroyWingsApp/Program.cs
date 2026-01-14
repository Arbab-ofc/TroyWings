using TroyWingsApp.Data;
using TroyWingsApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRegistrationRepository, MySqlRegistrationRepository>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var repository = scope.ServiceProvider.GetRequiredService<IRegistrationRepository>();
    repository.EnsureDatabaseSetup();
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
