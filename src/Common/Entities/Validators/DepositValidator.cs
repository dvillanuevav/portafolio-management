using FluentValidation;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities.Validators
{
    public class DepositValidator : AbstractValidator<Deposit>
    {
        public DepositValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty()
                .Must(amount => amount.IsGreaterThanZero())
                .WithMessage("It must be greather than zero.");

            RuleFor(x => x.TransactionDate)
                .NotEmpty()
                .Must(date => date.IsValidDate())
                .WithMessage("It must be format 'yyyy-MM-ddTHH:mm:ssK'");
        }
    }
}
