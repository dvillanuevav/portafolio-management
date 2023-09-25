using FluentValidation;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities.Validators
{
    public class SharePriceValidator : AbstractValidator<SharePrice>
    {
        public SharePriceValidator()
        {
            RuleFor(x => x.Price)
                .NotEmpty()
                .Must(amount => amount.IsGreaterThanZero())
                .WithMessage("It must be greater than zero.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .Must(date => date.IsValidDate("yyyy-MM-dd"))
                .WithMessage("It must be format 'yyyy-MM-dd'");
        }
    }
}
