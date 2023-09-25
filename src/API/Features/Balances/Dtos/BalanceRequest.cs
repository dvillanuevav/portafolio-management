using FluentValidation;
using Newtonsoft.Json;
using Common.Entities.Validators;

namespace API.Features.Balances.Dtos
{
    public class BalanceRequest
    {
        [JsonProperty("date")]
        public string Date { get; init; }
    }

    public class BalanceRequestValidator : AbstractValidator<BalanceRequest>
    {
        public BalanceRequestValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty()
                .Must(date => date.IsValidDate("yyyy-MM-dd"))
                .WithMessage("It must be format 'yyyy-MM-dd'");

        }
    }
}
