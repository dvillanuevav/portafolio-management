using API.Features.Balances.Dtos;
using FluentValidation;
using Newtonsoft.Json;
using Common.Entities.Validators;

namespace API.Features.SharePrices.Dtos
{
    public class SharePriceRequest
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }

    public class SharePriceRequestValidator : AbstractValidator<SharePriceRequest>
    {
        public SharePriceRequestValidator()
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
