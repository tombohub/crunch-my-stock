using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Crunch.Database.Models;

namespace Crunch.Database.Models
{
    public partial class stock_analyticsContext : DbContext
    {
        public stock_analyticsContext()
        {
        }

        public stock_analyticsContext(DbContextOptions<stock_analyticsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AverageRoi> AverageRois { get; set; }
        public virtual DbSet<Bottom10Stock> Bottom10Stocks { get; set; }
        public virtual DbSet<MultiplotCoordinate> MultiplotCoordinates { get; set; }
        public virtual DbSet<OvernightDailyStat> OvernightDailyStats { get; set; }
        public virtual DbSet<PricesDaily> PricesDailies { get; set; }
        public virtual DbSet<PricesIntraday> PricesIntradays { get; set; }
        public virtual DbSet<Security> Securities { get; set; }
        public virtual DbSet<SpyRoi> SpyRois { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<Top10Stock> Top10Stocks { get; set; }
        public virtual DbSet<WeeklyOvernightStat> WeeklyOvernightStats { get; set; }
        public virtual DbSet<WinnersLosersCount> WinnersLosersCounts { get; set; }
        public virtual DbSet<WinnersLosersCountByPrice> WinnersLosersCountByPrices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server=***REMOVED***;Port=5432;Database***REMOVED***;User Id=***REMOVED***;Password=***REMOVED***;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum("exchange", new[] { "AMEX", "NYSE", "NASDAQ" })
                .HasPostgresEnum("price_interval", new[] { "OneDay", "ThirtyMinutes" })
                .HasPostgresEnum("security_type", new[] { "Stock", "Etf", "Trust" });

            modelBuilder.Entity<AverageRoi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("average_roi", "overnight");

                entity.HasComment("Daily average overnight ROI for the strategy accross all securities. Value is in %");

                entity.Property(e => e.AverageRoi1).HasColumnName("average_roi");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();
            });

            modelBuilder.Entity<Bottom10Stock>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("bottom10_stocks", "overnight");

                entity.Property(e => e.AreaId).HasColumnName("area_id");

                entity.Property(e => e.ChangePct).HasColumnName("change_pct");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.EndPrice).HasColumnName("end_price");

                entity.Property(e => e.Rank).HasColumnName("rank");

                entity.Property(e => e.StartPrice).HasColumnName("start_price");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(4)
                    .HasColumnName("symbol");
            });

            modelBuilder.Entity<MultiplotCoordinate>(entity =>
            {
                entity.ToTable("multiplot_coordinates");

                entity.HasComment("Coordinates  and size for each report plot so it can be draw properly inside the code and create multiplot image.\r\n\r\nCoordinates x and y represen top left corner of the rectangle. Size is marked by 'width' and 'height'. width goes along x axis left to right and height along y axis top down.");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AreaName)
                    .IsRequired()
                    .HasColumnName("area_name")
                    .HasComment("name of the report which also corresponds with Enum in application code");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.IsIncluded)
                    .HasColumnName("is_included")
                    .HasComment("true if report is included in multiplot");

                entity.Property(e => e.Strategy)
                    .IsRequired()
                    .HasColumnName("strategy")
                    .HasComment("name of the strategy which also corresponds with Enum in application code");

                entity.Property(e => e.Width).HasColumnName("width");

                entity.Property(e => e.X).HasColumnName("x");

                entity.Property(e => e.Y).HasColumnName("y");
            });

            modelBuilder.Entity<OvernightDailyStat>(entity =>
            {
                entity.ToTable("overnight_daily_stats", "overnight");

                entity.HasIndex(e => new { e.Date, e.Symbol }, "overnight_daily_stats__date_symbol_un")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ChangePct)
                    .HasColumnName("change_pct")
                    .HasComputedColumnSql("round((((end_price - start_price) / start_price) * (100)::numeric), 2)", true)
                    .HasComment("Change between previous day close and today open price in %");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasComment("Date of the strategy");

                entity.Property(e => e.EndPrice)
                    .HasColumnName("end_price")
                    .HasComment("Strategy date opening price");

                entity.Property(e => e.StartPrice)
                    .HasColumnName("start_price")
                    .HasComment("Previous trading day closing price");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasColumnName("symbol");
            });

            modelBuilder.Entity<PricesDaily>(entity =>
            {
                entity.ToTable("prices_daily");

                entity.HasIndex(e => new { e.Symbol, e.Date }, "date_symbol_un")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Close).HasColumnName("close");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.High).HasColumnName("high");

                entity.Property(e => e.Interval)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("interval")
                    .HasDefaultValueSql("'OneDay'::character varying");

                entity.Property(e => e.Low).HasColumnName("low");

                entity.Property(e => e.Open).HasColumnName("open");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasColumnName("symbol");

                entity.Property(e => e.Volume).HasColumnName("volume");
            });

            modelBuilder.Entity<PricesIntraday>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("prices_intraday");

                entity.Property(e => e.Close).HasColumnName("close");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.High).HasColumnName("high");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Interval)
                    .HasMaxLength(10)
                    .HasColumnName("interval");

                entity.Property(e => e.Low).HasColumnName("low");

                entity.Property(e => e.Open).HasColumnName("open");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .HasColumnName("symbol");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("timestamp");

                entity.Property(e => e.Volume).HasColumnName("volume");
            });

            modelBuilder.Entity<Security>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("securities");

                entity.HasComment("List of available securities on market. Stocks and ETFs");

                entity.HasIndex(e => e.Symbol, "securities_symbol_un")
                    .IsUnique();

                entity.Property(e => e.DelistingDate)
                    .HasColumnName("delisting_date")
                    .HasComment("date when security was delisted, if delisted");

                entity.Property(e => e.Exchange)
                    .IsRequired()
                    .HasColumnName("exchange");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.IpoDate)
                    .HasColumnName("ipo_date")
                    .HasComment("date of initial public offering");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasComment("active or delisted");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasColumnName("symbol");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<SpyRoi>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("spy_roi", "overnight");

                entity.HasComment("Daily SPY ROI for the strategy accross all securities. Value is in %");

                entity.Property(e => e.AreaId).HasColumnName("area_id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.SpyRoi1).HasColumnName("spy_roi");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tasks");

                entity.HasComment("Tasks to be run periodically");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Frequency).HasColumnName("frequency");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Task1)
                    .IsRequired()
                    .HasColumnName("task");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("test");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Str).HasColumnName("str");
            });

            modelBuilder.Entity<Top10Stock>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("top10_stocks", "overnight");

                entity.HasComment("Top 10 stocks each day according to performance(change_pct)");

                entity.Property(e => e.AreaId).HasColumnName("area_id");

                entity.Property(e => e.ChangePct)
                    .HasColumnName("change_pct")
                    .HasComment("Change between previous day close and today open price in %");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasComment("Date of the strategy");

                entity.Property(e => e.EndPrice)
                    .HasColumnName("end_price")
                    .HasComment("Strategy date opening price");

                entity.Property(e => e.Rank).HasColumnName("rank");

                entity.Property(e => e.StartPrice)
                    .HasColumnName("start_price")
                    .HasComment("Previous trading day closing price");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(4)
                    .HasColumnName("symbol");
            });

            modelBuilder.Entity<WeeklyOvernightStat>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("weekly_overnight_stats", "overnight");

                entity.HasIndex(e => new { e.Symbol, e.Strategy, e.WeekNum }, "symbol_strategy_week_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.AvgDay).HasColumnName("avg_day");

                entity.Property(e => e.AvgLossPerLosingTrade).HasColumnName("avg_loss_per_losing_trade");

                entity.Property(e => e.AvgPctGainPerTrade).HasColumnName("avg_pct_gain_per_trade");

                entity.Property(e => e.AvgProfitPerTrade).HasColumnName("avg_profit_per_trade");

                entity.Property(e => e.AvgProfitPerWinningTrade).HasColumnName("avg_profit_per_winning_trade");

                entity.Property(e => e.AvgWeeklyClosedOutDrawdown).HasColumnName("avg_weekly_closed_out_drawdown");

                entity.Property(e => e.AvgWeeklyClosedOutRunup).HasColumnName("avg_weekly_closed_out_runup");

                entity.Property(e => e.BeginningBalance).HasColumnName("beginning_balance");

                entity.Property(e => e.BestDay).HasColumnName("best_day");

                entity.Property(e => e.DailyStd).HasColumnName("daily_std");

                entity.Property(e => e.End).HasColumnName("end");

                entity.Property(e => e.EndingBalance).HasColumnName("ending_balance");

                entity.Property(e => e.GrossLoss).HasColumnName("gross_loss");

                entity.Property(e => e.GrossProfit).HasColumnName("gross_profit");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.LargestLossLosingTrade).HasColumnName("largest_loss_losing_trade");

                entity.Property(e => e.LargestPctLosingTrade).HasColumnName("largest_pct_losing_trade");

                entity.Property(e => e.LargestPctWinningTrade).HasColumnName("largest_pct_winning_trade");

                entity.Property(e => e.LargestProfitWinningTrade).HasColumnName("largest_profit_winning_trade");

                entity.Property(e => e.MaxClosedOutDrawdown).HasColumnName("max_closed_out_drawdown");

                entity.Property(e => e.MaxConsecutiveLosingTrades).HasColumnName("max_consecutive_losing_trades");

                entity.Property(e => e.MaxConsecutiveWinningTrades).HasColumnName("max_consecutive_winning_trades");

                entity.Property(e => e.MaxWeeklyClosedOutDrawdown).HasColumnName("max_weekly_closed_out_drawdown");

                entity.Property(e => e.MaxWeeklyClosedOutRunup).HasColumnName("max_weekly_closed_out_runup");

                entity.Property(e => e.NumEvenTrades).HasColumnName("num_even_trades");

                entity.Property(e => e.NumLosingTrades).HasColumnName("num_losing_trades");

                entity.Property(e => e.NumWinningTrades).HasColumnName("num_winning_trades");

                entity.Property(e => e.PctProfitableTrades).HasColumnName("pct_profitable_trades");

                entity.Property(e => e.ProfitFactor).HasColumnName("profit_factor");

                entity.Property(e => e.RatioAvgProfitWinLoss).HasColumnName("ratio_avg_profit_win_loss");

                entity.Property(e => e.ReturnOnInitialCapital).HasColumnName("return_on_initial_capital");

                entity.Property(e => e.SecurityType)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("security_type");

                entity.Property(e => e.SharpeRatio).HasColumnName("sharpe_ratio");

                entity.Property(e => e.SharpeRatioMax).HasColumnName("sharpe_ratio_max");

                entity.Property(e => e.SharpeRatioMin).HasColumnName("sharpe_ratio_min");

                entity.Property(e => e.SortinoRatio).HasColumnName("sortino_ratio");

                entity.Property(e => e.Start).HasColumnName("start");

                entity.Property(e => e.StartPrice).HasColumnName("start_price");

                entity.Property(e => e.Strategy)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("strategy");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasColumnName("symbol");

                entity.Property(e => e.TotalNetProfit).HasColumnName("total_net_profit");

                entity.Property(e => e.WeekNum).HasColumnName("week_num");

                entity.Property(e => e.WorstDay).HasColumnName("worst_day");
            });

            modelBuilder.Entity<WinnersLosersCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("winners_losers_count", "overnight");

                entity.HasComment("Report counting how many stocks were up overnight (winners) how many down (losers)");

                entity.Property(e => e.AreaId).HasColumnName("area_id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.LosersCount).HasColumnName("losers_count");

                entity.Property(e => e.WinnersCount).HasColumnName("winners_count");
            });

            modelBuilder.Entity<WinnersLosersCountByPrice>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("winners_losers_count_by_price", "overnight");

                entity.HasComment("Number of winning and losing securities grouped by custom price ranges");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.GroupOrder).HasColumnName("group_order");

                entity.Property(e => e.LosersCount).HasColumnName("losers_count");

                entity.Property(e => e.PriceRange).HasColumnName("price_range");

                entity.Property(e => e.WinnersCount).HasColumnName("winners_count");
            });

            modelBuilder.HasSequence("id");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
