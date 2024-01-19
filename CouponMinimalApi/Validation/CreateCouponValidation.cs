using CouponMinimalApi.Models.Params;
using FluentValidation;

namespace CouponMinimalApi.Validation
{
    public class CreateCouponValidation : AbstractValidator<CreateCouponRequest>
    {
        public CreateCouponValidation() 
        {
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Percent).InclusiveBetween(1, 100);
        }
    }
}
