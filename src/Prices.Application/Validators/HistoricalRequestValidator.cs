using FluentValidation;
using Prices.Contracts.Requests;

namespace Prices.Application.Validators
{
    public class HistoricalRequestValidator : AbstractValidator<HistoricalPricesRequest>
    {
        public HistoricalRequestValidator()
        {
            RuleFor(x => x.Instrument)
                .NotEmpty().WithMessage("Instrument cannot be empty.")
                .Matches("^[a-z]+$").WithMessage("Instrument must be lowercase letters only.")
                .MustAsync(ValidateInstrument).WithMessage("Instrument is not available.");

            RuleFor(x => x.startTime)
                .GreaterThan(0)
                .LessThanOrEqualTo(9999999999).WithMessage("Milliseconds timestamps are not allowed.")
                .Must(ValidateDate).WithMessage("Time should be rounded to hours.");

            RuleFor(x => x.endTime)
                .GreaterThan(x => x.startTime).WithMessage("End time should be greater than start.")
                .GreaterThan(0)
                .LessThanOrEqualTo(9999999999).WithMessage("Milliseconds timestamps are not allowed.")
                .Must(ValidateDate).WithMessage("Time should be rounded to hours.");
        }

        private bool ValidateDate(long timePoint)
        {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(timePoint).UtcDateTime;

            return dateTime.Minute == 0 && dateTime.Second == 0;
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
