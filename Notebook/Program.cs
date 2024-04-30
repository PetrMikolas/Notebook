using Microsoft.AspNetCore.Identity;
using Notebook.Api.ErrorClient;
using Notebook.Api.Notebook;
using Notebook.Client.Services.Api;
using Notebook.Components;
using Notebook.Databases.Notebook;
using Notebook.Mappers;
using Notebook.Services.Email;
using Notebook.Services.Notebook;
using Radzen;
using Notebook.Models;
using Notebook.Services.User;
using Notebook.Services.Identity;
using Notebook.Account;
using static Notebook.Services.Email.EmailService;
using Microsoft.Extensions.Options;
using Notebook.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry(o =>
{
	o.Dsn = builder.Configuration["SentryDsn"]!;
	o.Debug = false;    
    o.TracesSampleRate = 1.0;
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddIdentityServices();
builder.Services.AddTransient<UserMiddleware>();
builder.Services.AddMemoryCache();

builder.Services
    .AddOptions<EmailOptions>()
    .BindConfiguration(EmailOptions.Key)    
    .ValidateDataAnnotations();

builder.Services.AddScoped<IEmailSender<AppUser>, EmailSender>();

var loggerEmailService = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<EmailService>();
var optionsEmailService = Options.Create(new EmailOptions());
var emailService = new EmailService(optionsEmailService, loggerEmailService);

// Database
builder.Services.AddDatabaseNotebook(builder.Configuration, emailService);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDocument();

builder.Services.AddAutoMapper(config =>
    config.AddProfile<AutoMapperProfile>(), typeof(Program).Assembly);

builder.Services.AddTransient<INotebookService, NotebookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Client service
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddHttpClient<IApiClient, ApiClient>(config =>
    config.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!));

var app = builder.Build();

app.UseUserMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Notebook.Client._Imports).Assembly);

// Database
app.UseDatabaseNotebook(emailService);

// Add additional endpoints required by the Identity 
app.MapAdditionalIdentityEndpoints();

// minimal API
app.MapEndpointsNotebook();
app.MapEndpointsClientErrors();

app.Run();