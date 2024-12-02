namespace Prices.Api
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Prices
        {
            private const string Base = $"{ApiBase}/prices";

            public const string Aggregated = $"{Base}/{{Instrument}}/aggregated";
            public const string Historical = $"{Base}/{{Instrument}}/historical";
        }
    }
}
