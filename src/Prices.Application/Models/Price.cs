﻿namespace Prices.Application.Models
{
    public partial class Price
    {
        public required Guid Id { get; init; }
        public required string Provider { get; set; }
        public required string Instrument { get; set; }
        public required long TimePoint { get; set; }
        public required double Close { get; set; }
    }
}
