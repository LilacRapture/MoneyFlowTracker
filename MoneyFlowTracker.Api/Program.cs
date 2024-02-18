using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Api.Domain.Category;
using MoneyFlowTracker.Api.Domain.Item;
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
// builder.Services.AddAutoMapper(typeof(InspirerMapperProfile));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(Program).Assembly,
    MoneyFlowTrackerBusinessAssembly.GetAssembly()
));

// Custom Services
builder.Services.AddTransient<IDataContext, MoneyFlowTrackerDbContext>();

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
app.MapPut("/api/items/", UpdateItemQueryApi.Handler);
app.MapPost("/api/items/", AddItemQueryApi.Handler);

// Category
app.MapGet("/api/categories", GetAllCategoriesQueryApi.Handler);

// Analytics
app.MapGet("/api/analytics/{date}", GetAnalyticsQueryApi.Handler);
app.MapGet("/api/analytics/chart", GetAnalyticsChartQueryApi.Handler);


app.Run();
