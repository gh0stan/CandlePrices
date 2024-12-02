using FluentValidation;
using Prices.Contracts.Requests;

namespace Prices.Application.Validators
{
    public class PriceRequestValidator : AbstractValidator<AggregatedPriceRequest>
    {
        public PriceRequestValidator()
        {
            RuleFor(x => x.Instrument)
                .NotEmpty().WithMessage("Instrument cannot be empty.")
                .Matches("^[a-z]+$").WithMessage("Instrument must be lowercase letters only.")
                .MustAsync(ValidateInstrument).WithMessage(x => $"Instrument is not available. You have entered: {x.Instrument}");

            RuleFor(x => x.TimePoint)
                .GreaterThan(0)
                .LessThanOrEqualTo(9999999999).WithMessage("Milliseconds timestamps are not allowed.")
                .Must(ValidateDate)
                .WithMessage("Time should be rounded to hours.");
        }

        private bool ValidateDate(long timePoint)
        {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(timePoint).UtcDateTime;

            if (dateTime.Minute == 0 && dateTime.Second == 0)
            {
                return true;
            }

            return false;
        }

        private Task<bool> ValidateInstrument(string instrument, CancellationToken token = default)
        {
            var availableFromDb = new List<string>
            {
                "btcusd"
            };
            
            if (availableFromDb.Contains(instrument))
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
