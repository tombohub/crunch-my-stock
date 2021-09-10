using System;
using System.Collections.Generic;

#nullable disable

namespace Crunch.Infrastructure.Database.Models
{
    public partial class WeeklyOvernightStat
    {
        public long Id { get; set; }
        public string Symbol { get; set; }
        public string SecurityType { get; set; }
        public string Strategy { get; set; }
        public double StartPrice { get; set; }
        public long WeekNum { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public long BeginningBalance { get; set; }
        public double EndingBalance { get; set; }
        public double TotalNetProfit { get; set; }
        public double GrossProfit { get; set; }
        public double GrossLoss { get; set; }
        public double? ProfitFactor { get; set; }
        public double ReturnOnInitialCapital { get; set; }
        public long? NumWinningTrades { get; set; }
        public long? NumLosingTrades { get; set; }
        public long? NumEvenTrades { get; set; }
        public double? PctProfitableTrades { get; set; }
        public double? AvgProfitPerTrade { get; set; }
        public double? AvgProfitPerWinningTrade { get; set; }
        public double? AvgLossPerLosingTrade { get; set; }
        public double? RatioAvgProfitWinLoss { get; set; }
        public double? LargestProfitWinningTrade { get; set; }
        public double? LargestLossLosingTrade { get; set; }
        public double? AvgPctGainPerTrade { get; set; }
        public double? LargestPctWinningTrade { get; set; }
        public double? LargestPctLosingTrade { get; set; }
        public long? MaxConsecutiveWinningTrades { get; set; }
        public long? MaxConsecutiveLosingTrades { get; set; }
        public double? MaxClosedOutDrawdown { get; set; }
        public double? AvgWeeklyClosedOutDrawdown { get; set; }
        public double? MaxWeeklyClosedOutDrawdown { get; set; }
        public double? AvgWeeklyClosedOutRunup { get; set; }
        public double? MaxWeeklyClosedOutRunup { get; set; }
        public double? BestDay { get; set; }
        public double? WorstDay { get; set; }
        public double? AvgDay { get; set; }
        public double? DailyStd { get; set; }
        public double? SharpeRatio { get; set; }
        public double? SharpeRatioMax { get; set; }
        public double? SharpeRatioMin { get; set; }
        public double? SortinoRatio { get; set; }
    }
}
