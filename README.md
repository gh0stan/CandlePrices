# PricesService

Micro-service for aggregation, retrieval & caching of financial instruments prices.__
A simple use case is that a user requests the bitcoin price (BTC/USD) at a specific time-point, micro-service fetches the prices from multiple sources, aggregate them using some logic, then return it to the client. Aggregated price persisted in local storage.__
Another option allows getting historic data between timepoints.__

APIs request the 1h OHLC candle (open, high, low, close - 1 hour window) with financial instrument name specified.__
From their response this service currently only uses close price field, which is the price at the requested time point and aggregates multiple prices to a single one (average).__
Historic request display same information but for every 1hour in between timepoints.__

Database: postgres (docker-file provided).__
Cache: .NET Output Caching.__
Current available instruments: only BTC/USD. (enter lowercase, no symbols: btcusd).__
Time formats: unixtime, have hour-accuracy (no minutes/seconds allowed, hour = 3600, format unixtime: 1672531200).__

Price sources:__
1. Bitstamp API endpoint: https://www.bitstamp.net/api/#tag/Market-info/operation/GetOHLCData__
example: https://www.bitstamp.net/api/v2/ohlc/btcusd/?step=3600&limit=1&start=1672531200__
2. Bitfinex API endpoint: https://docs.bitfinex.com/reference/rest-public-candles#__
example: https://api-pub.bitfinex.com/v2/candles/trade:1h:tBTCUSD/hist?start=1672531200000&end=1672534800000&limit=1__
