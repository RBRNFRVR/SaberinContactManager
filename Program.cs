using ContactManager.Data;
using ContactManager.Hubs;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using ElmahCore.Mvc;
using ElmahCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddDbContext<ApplicationContext>(options =>
                options
                    .UseSqlServer(builder.Configuration.GetConnectionString("ContactDb"),
                        opts => opts.CommandTimeout(600)));

builder.Services.AddSignalR();

builder.Services.AddElmah<XmlFileErrorLog>(options =>
{
    options.Path = "/elmah";
    options.LogPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dataContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseElmah();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ContactHub>("/contacthub");

app.Run();
