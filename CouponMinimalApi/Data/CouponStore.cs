using CouponMinimalApi.Models;

namespace CouponMinimalApi.Data
{
    public static class CouponStore
    {
        public static List<Coupon> Coupons = new()
        {
            new Coupon(){Id = 1, Name="One", Percent=20, CreatedOn=DateTime.Now,UpdatedOn=DateTime.Now},
            new Coupon(){Id = 2, Name="Two", Percent=30, CreatedOn=DateTime.Now,UpdatedOn=DateTime.Now}
        };
    }
}
