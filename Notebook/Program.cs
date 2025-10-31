using Microsoft.AspNetCore.Identity;
using Notebook.Api.ErrorClient;
using Notebook.Api.Notebook;
using Notebook.Client.Services.Api;
using Notebook.Components;
using Notebook.Databases.Notebook;
using Notebook.Mappers;
using Notebook.Services.Email;
using Notebook.Services.Notebook;
using Notebook.Services.User;
using Notebook.Services.Identity;
using Notebook.Account;
using static Notebook.Services.Email.EmailService;
using Notebook.Sentry;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.DataProtection;
using Notebook.Models;
using Notebook.Middlewares;
using Radzen;

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

// Create and configure email service instance.
var loggerEmailService = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<EmailService>();
var optionsEmailService = Options.Create(new EmailOptions());
var emailService = new EmailService(optionsEmailService, loggerEmailService);

// Add Data Protection with persistent keys
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/app/Keys"))
    .SetApplicationName("notebook");

// Register database-related services.
builder.Services.AddNotebookDatabase(builder.Configuration, emailService);

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

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// Configure Razor Components.
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Notebook.Client._Imports).Assembly);

// Apply database migrations.
app.UseNotebookDatabase(emailService);

// Map application endpoints.  
app.MapAdditionalIdentityEndpoints();
app.MapNotebookEndpoints();
app.MapClientErrorEndpoints();

app.Run();