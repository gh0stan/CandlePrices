using Prices.Contracts.Dto;

namespace Prices.Contracts.Responses
{
    public record HistoricalPriceResponse(
        string Instrument,
        List<PricePointDto> Prices
    );
}
