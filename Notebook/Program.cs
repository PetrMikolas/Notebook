using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Notebook.Account;
using Notebook.Api.ErrorClient;
using Notebook.Api.Notebook;
using Notebook.Client.Services.Api;
using Notebook.Components;
using Notebook.Databases.Notebook;
using Notebook.Mappers;
using Notebook.Middlewares;
using Notebook.Models;
using Notebook.Sentry;
using Notebook.Services.Email;
using Notebook.Services.Identity;
using Notebook.Services.Notebook;
using Notebook.Services.User;
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
builder.Services.AddTransient<UserMiddleware>();
builder.Services.AddMemoryCache();

// Register API explorer and Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDocument();

// Register application services.
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailSender<AppUser>, EmailSender>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<INotebookService, NotebookService>();

// Add IHttpContextAccessor to the container.
builder.Services.AddHttpContextAccessor();

// Register client services and configure HttpClient for ApiClient.
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddHttpClient<IApiClient, ApiClient>(config =>
    config.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!));

var app = builder.Build();

// Apply user middleware.
app.UseUserMiddleware();

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
    app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

// Not used in Docker – HTTPS is handled by the proxy (avoids warning or redirect loop)
//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

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