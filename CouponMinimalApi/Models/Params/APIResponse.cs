using System.Net;

namespace CouponMinimalApi.Models.Params
{
    public class APIResponse
    {
        public bool IsSuccess {  get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object Results { get; set; }
        public string ErrorMessage { get; set; }
    }
}
