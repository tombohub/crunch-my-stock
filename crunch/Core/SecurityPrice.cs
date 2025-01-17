﻿namespace Crunch.Core
{
    /// <summary>
    /// Security 1 day interval OHLC price data 
    /// </summary>
    public record SecurityPrice
    {
        public required Symbol Symbol { get; init; }
        public required SecurityType SecurityType { get; init; }
        public required TradingDay TradingDay { get; init; }
        public required OHLC OHLC { get; init; }
        public required long Volume { get; init; }
    }

}