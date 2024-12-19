using FlightSearch.Application.Interfaces;
using FlightSearch.Application.Services;
using FlightSearch.Domain.Entities;
using FlightSearch.Domain.Interfaces;
using FlightSearch.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddDistributedMemoryCache();

builder.Services.Configure<AmadeusApiSettings>(builder.Configuration.GetSection("AmadeusApiSettings"));

builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
builder.Services.AddSingleton<IAmadeusApiService, AmadeusApiService>();
builder.Services.AddSingleton<IFlightsSearchService, FlightsSearchService>();
builder.Services.AddSingleton<IAmadeusOAuthClient, AmadeusOAuthClient>();
builder.Services.AddSingleton<IAmadeusTokenCache, AmadeusTokenCache>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular app origin
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAngularApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAnyOrigin");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();