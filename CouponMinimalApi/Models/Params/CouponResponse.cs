namespace CouponMinimalApi.Models.Params
{
    public class CouponResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percent { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
