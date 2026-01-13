using TroyWingsApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRegistrationRepository, MySqlRegistrationRepository>();
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
