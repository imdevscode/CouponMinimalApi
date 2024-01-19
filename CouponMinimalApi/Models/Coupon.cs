namespace CouponMinimalApi.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percent { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.Today;
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
