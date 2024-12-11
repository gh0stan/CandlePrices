namespace Prices.Contracts.Responses
{
    public record PriceResponse(
        string Instrument, 
        long TimePoint,
        double AggregatedPrice
    );
}
