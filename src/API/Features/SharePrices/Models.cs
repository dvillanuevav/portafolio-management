using FluentValidation;
using Newtonsoft.Json;
using Common.Entities.Validators;

namespace API.Features.SharePrices
{
    internal sealed class Request
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
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

    internal struct Response
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
