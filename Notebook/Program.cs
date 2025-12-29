using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Notebook.Account;
using Notebook.Api.ErrorClient;
using Notebook.Api.Notebook;
using Notebook.Client.Services.Api;
using Notebook.Components;
using Notebook.Databases.Notebook;
using Notebook.Mappers;
using Notebook.Models;
using Notebook.Sentry;
using Notebook.Services.Email;
using Notebook.Services.Identity;
using Notebook.Services.Notebook;
using Radzen;
using static Notebook.Services.Email.EmailService;

var builder = WebApplication.CreateBuilder(args);

// Add Sentry services to the WebHostBuilder.
builder.WebHost.AddSentry(builder.Configuration);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Register and configure AutoMapper with custom profile.
builder.Services.AddAutoMapper(config =>
    config.AddProfile<AutoMapperProfile>(), typeof(Program).Assembly);

// Configure options for email service.
builder.Services
    .AddOptions<EmailOptions>()
    .BindConfiguration(EmailOptions.Key)
    .ValidateDataAnnotations();

// Add Data Protection with persistent keys
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/app/Keys"))
    .SetApplicationName("notebook");

// Register database-related services.
builder.Services.AddNotebookDatabase(builder.Configuration);

builder.Services.AddIdentityServices();
builder.Services.AddMemoryCache();

// Register API explorer and Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDocument();

// Register application services.
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailSender<AppUser>, EmailSender>();
builder.Services.AddTransient<INotebookService, NotebookService>();

// Register client services and configure HttpClient for ApiClient.
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddHttpClient<IApiClient, ApiClient>(config =>
    config.BaseAddress = new Uri(builder.Configuration["BaseUrl"]!));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable debugging and Swagger UI in development.
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Handle exceptions and enforce HTTPS in production.
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts(); 
}

// Not used in Docker – HTTPS is handled by the proxy (avoids warning or redirect loop)
//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

if (Environment.GetEnvironmentVariable("NSWAG_RUNNING") is not "true")
    app.MapStaticAssets();

// Configure Razor Components.
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Notebook.Client._Imports).Assembly);

// Apply database migrations.
await app.UseNotebookDatabaseAsync();

// Map application endpoints.  
app.MapAdditionalIdentityEndpoints();
app.MapNotebookEndpoints();
app.MapClientErrorEndpoints();

app.Run();