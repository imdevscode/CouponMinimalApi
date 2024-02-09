using AutoMapper;
using CouponMinimalApi.Data;
using CouponMinimalApi.Models;
using CouponMinimalApi.Models.Params;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CouponMinimalApi.Endpoints
{
    public static class CouponEndpoints
    {
        public static void ConfigureCouponEndpoints(this WebApplication webApplication)
        {
            webApplication.MapGet("api/allcoupons", GetAllCoupon)
                .WithName("GetAllCoupon").Produces<List<Coupon>>(200);

            webApplication.MapGet("api/coupon/{id:int}", GetCouponById)
                .WithName("GetCouponById").Produces<APIResponse>(200).Produces(404);

            webApplication.MapPost("api/coupon", CreateCoupon)
                .WithName("CreateCoupon").Accepts<CreateCouponRequest>("application/json").Produces<APIResponse>(200).Produces(400);
        }

        private static IResult GetAllCoupon(ILogger<Program> _logger)
        {
            _logger.Log(LogLevel.Information, $"Fetching All coupons data.");
            return Results.Ok(CouponStore.Coupons);
        }

        private static IResult GetCouponById(ILogger<Program> _logger, int id) 
        {
            APIResponse response = new()
            { IsSuccess = false, StatusCode = HttpStatusCode.NotFound };
            
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
        }

        private async static Task<IResult> CreateCoupon(ILogger<Program> _logger, IMapper _mapper,
                                        IValidator<CreateCouponRequest> _validator,
                                        [FromBody] CreateCouponRequest request)
        {
            _logger.Log(LogLevel.Information, "Calling CreateCoupon");
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
        }
    }
}
