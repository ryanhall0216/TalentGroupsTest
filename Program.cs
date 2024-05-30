using TalentGroupsTest.Components;
using TalentGroupsTest.Models;
using Microsoft.Extensions.Options;
using TalentGroupsTest.Data.Repository.Interface;
using TalentGroupsTest.Data.Repository;
using TalentGroupsTest.Data.Services.Interface;
using TalentGroupsTest.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddCircuitOptions(options => { options.DetailedErrors = true; });

// MongDB connection
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<IMongoDbSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
builder.Services.AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>));

builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
