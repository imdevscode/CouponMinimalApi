using AutoMapper;
using CouponMinimalApi.Models;
using CouponMinimalApi.Models.Params;

namespace CouponMinimalApi.Mapping
{
    public class CouponMappingProfile : Profile
    {
        public CouponMappingProfile() 
        {
            CreateMap<Coupon,CreateCouponRequest>().ReverseMap();
            CreateMap<Coupon,CouponResponse>().ReverseMap();
        }
    }
}
