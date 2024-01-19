using AutoMapper;
using CouponMinimalApi.Data;
using CouponMinimalApi.Mapping;
using CouponMinimalApi.Models;
using CouponMinimalApi.Models.Params;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(CouponMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/helloworld", () => "Hello World");
app.MapGet("/api/coupons", () => Results.Ok(CouponStore.Coupons)).WithName("GetCoupons");

app.MapGet("/api/coupon/{id:int}", (ILogger<Program> _logger, int id) =>
{
    _logger.Log(LogLevel.Information, $"Fetching Coupon for id = {id}");
    if (!CouponStore.Coupons.Any(x => x.Id == id))
        return Results.NotFound("Coupon doesn't exist");
    Coupon coupon = CouponStore.Coupons.FirstOrDefault(x => x.Id == id);
    return Results.Ok(coupon);
}).WithName("GetCoupon").Produces<Coupon>(200).Produces(404);

app.MapPost("/api/CreateCoupon", (IMapper _mapper, [FromBody] CreateCouponRequest request) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
    if (string.IsNullOrEmpty(request.Name))
    {
        response.ErrorMessage = "Counpon name invalid";
        return Results.BadRequest(response);
    }
    if (CouponStore.Coupons.Any(x => x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
    {
        response.ErrorMessage = $"Coupon {request.Name} already exist!";
        return Results.BadRequest(response);
    }
    Coupon coupon = _mapper.Map<Coupon>(request);

    coupon.Id = CouponStore.Coupons.OrderByDescending(x => x.Id).First().Id + 1;
    CouponStore.Coupons.Add(coupon);
    CouponResponse couponResponse = _mapper.Map<CouponResponse>(coupon);

    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.Created;
    response.Results = couponResponse;

    return Results.Ok(response);
}).WithName("CreateCoupon").Accepts<CreateCouponRequest>("application/json").Produces<APIResponse>(200).Produces(400);

app.UseHttpsRedirection();


app.Run();
