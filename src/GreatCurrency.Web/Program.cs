using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Repository;
using GreatCurrency.BLL.Services;
using GreatCurrency.DAL.Context;
using GreatCurrency.Web.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


string connection = builder.Configuration.GetConnectionString("GreatCurrencyPostgreSQL");
var mainBank = builder.Configuration["AppSettings:MainBank"];
var mainService = builder.Configuration["AppSettings:MainService"];
var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
BotConfiguration.ChatId = builder.Configuration.GetSection("BotConfiguration:ChatId").Value;
var myfinAPILogin = builder.Configuration["APISettings:Login"];
var myfinAPIPassword = builder.Configuration["APISettings:Password"];
var statusbankAPILogin = builder.Configuration["StatusbankAPISettings:Login"];
var statusbankAPIPassword = builder.Configuration["StatusbankAPISettings:Password"];

builder.Services.AddDbContext<GreatCurrencyContext>(options =>
 options.UseNpgsql(connection));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IBankDepartmentService, BankDepartmentService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ISaveCurrencyService, SaveCurrencyService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IBestCurrencyService, BestCurrencyService>();
builder.Services.AddScoped<IBestRatesCounterService, BestRatesCounterService>();
builder.Services.AddTelegramBotClient(botConfigurationSection);
builder.Services.AddScoped(s => new GetParameters(mainBank, mainService));
builder.Services.AddScoped(s => new GetMyfinAPIParameters(myfinAPILogin, myfinAPIPassword));
builder.Services.AddScoped(s => new GetStatusbankParameters(statusbankAPILogin, statusbankAPIPassword));
builder.Services.AddScoped<ICheckCurrency, CheckCurrency>();
builder.Services.AddScoped<ISCRequestService, SCRequestService>();
builder.Services.AddScoped<IServiceCurrencyService, ServiceCurrencyService>();
builder.Services.AddScoped<ICSCurrencyService, CSCurrencyService>();
builder.Services.AddScoped<ICurrencyServiceCounterService, CurrencyServiceCounterService>();
builder.Services.AddScoped<IMyfinAPIService, MyfinAPIService>();
builder.Services.AddScoped<ISaveMyfinAPICurrencyService, SaveMyfinAPICurrencyService>();
builder.Services.AddScoped<IStatusBankAPIService,StatusBankAPIService>();
builder.Services.AddScoped<ILECurrencyService, LECurrencyService>();
builder.Services.AddScoped<ILEOrganisationService, LEOrganisationService>();
builder.Services.AddScoped<ILERequestService, LERequestService>();
builder.Services.AddScoped<IGetLegalCurrencyService, GetLegalCurrencyService>();
builder.Services.AddScoped<ILegalCurrencyCounterService, LegalCurrencyCounterService>();


builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(connection));
builder.Services.AddHangfireServer();
builder.Services.AddControllersWithViews();


var app = builder.Build();
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (env == "Development")
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseHangfireDashboard("/myhangfiredashboard");
app.Map("/users", () => "Users Page");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
