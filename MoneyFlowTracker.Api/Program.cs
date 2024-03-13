using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Api.Domain.Balance;
using MoneyFlowTracker.Api.Domain.Category;
using MoneyFlowTracker.Api.Domain.Item;
using MoneyFlowTracker.Api.Domain.NetItem;
using MoneyFlowTracker.Api.Util.Mapper;
using MoneyFlowTracker.Business.Domain.Chart.Services;
using MoneyFlowTracker.Business.Util;
using MoneyFlowTracker.Business.Util.Data;
using MoneyFlowTracker.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// DB
builder.Services.AddDbContextPool<MoneyFlowTrackerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Dev"))
);
// Mapper
builder.Services.AddAutoMapper(typeof(MoneyFlowTrackerMapperProfile));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(Program).Assembly,
    MoneyFlowTrackerBusinessAssembly.GetAssembly()
));

// Custom Services
builder.Services.AddTransient<IDataContext, MoneyFlowTrackerDbContext>();
builder.Services.AddTransient<IAnalyticsChartBuilder, AnalyticsChartBuilder>();
builder.Services.AddTransient<ICustomIncomeService, CustomIncomeService>();
builder.Services.AddTransient<IAnalyticsRowBuilder, AnalyticsRowBuilder>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Item
app.MapGet("/api/items/{date}", GetItemsByDateQueryApi.Handler);
app.MapGet("/api/items/{id:Guid}", GetItemQueryApi.Handler);
app.MapPost("/api/items/", UpsertItemQueryApi.Handler);

// Net Item
app.MapGet("/api/items/net/{date}", GetNetItemsByDateQueryApi.Handler);
app.MapGet("/api/items/net/{id:Guid}", GetNetItemQueryApi.Handler);
app.MapPost("/api/items/net/", UpsertNetItemQueryApi.Handler);

// Balance
app.MapGet("/api/balance/", GetCurrentBalanceQueryApi.Handler);
app.MapPost("/api/balance/", AddBalanceCommandApi.Handler);

// Category
app.MapGet("/api/categories", GetAllCategoriesQueryApi.Handler);

// Analytics
app.MapGet("/api/analytics/{date}", GetAnalyticsQueryApi.Handler);
app.MapGet("/api/analytics/chart", GetAnalyticsChartQueryApi.Handler);


app.Run();
