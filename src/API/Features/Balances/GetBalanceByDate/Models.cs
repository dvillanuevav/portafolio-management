using FluentValidation;
using Newtonsoft.Json;
using Common.Entities.Validators;

namespace API.Features.Balances.GetBalanceByDate
{
    internal sealed class Request
    {
        [JsonProperty("date")]
        public string Date { get; init; }
    }

    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {
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
        public string Date { get; init; }

        [JsonProperty("grandTotalShares")]
        public decimal GrandTotalShares { get; set; }

        [JsonProperty("grandTotal")]
        public decimal GrandTotal { get; set; }
    }
}