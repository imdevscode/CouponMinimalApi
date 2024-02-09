using AutoMapper;
using CouponMinimalApi.Data;
using CouponMinimalApi.Mapping;
using CouponMinimalApi.Models;
using CouponMinimalApi.Models.Params;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FluentValidation;
using CouponMinimalApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(CouponMappingProfile));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/helloworld", () => "Hello World");
/*
app.MapGet("/api/coupons", () => Results.Ok(CouponStore.Coupons)).WithName("GetCoupons");

app.MapGet("/api/coupon/{id:int}", (ILogger<Program> _logger, int id) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.NotFound };
    _logger.Log(LogLevel.Information, $"Fetching Coupon for id = {id}");
    if (!CouponStore.Coupons.Any(x => x.Id == id))
    {
        response.ErrorMessage = $"Coupon with Id = {id} doesn't exist";
        return Results.NotFound(response);
    }
    Coupon coupon = CouponStore.Coupons.FirstOrDefault(x => x.Id == id);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    response.Results = coupon;
    return Results.Ok(response);
}).WithName("GetCoupon").Produces<APIResponse>(200).Produces(404);

app.MapPost("/api/CreateCoupon", async (IMapper _mapper,
                                        IValidator<CreateCouponRequest> _validator, 
                                        [FromBody] CreateCouponRequest request) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
    var validation = await _validator.ValidateAsync(request);
    //if (string.IsNullOrEmpty(request.Name))
    if (!validation.IsValid)    
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
*/
app.ConfigureCouponEndpoints();
app.UseHttpsRedirection();


app.Run();
