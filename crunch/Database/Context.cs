using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Crunch.Database.Models;


#nullable disable

namespace Crunch.Database
{
    public partial class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Crametorium> Crametoria { get; set; }
        public virtual DbSet<GappersStat> GappersStats { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupsDailyOverview> GroupsDailyOverviews { get; set; }
        public virtual DbSet<IntranightStat> IntranightStats { get; set; }
        public virtual DbSet<Price> Prices { get; set; }
        public virtual DbSet<Prices1d> Prices1ds { get; set; }
        public virtual DbSet<Prices30m> Prices30ms { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Security> Securities { get; set; }
        public virtual DbSet<WeeklyOvernightStat> WeeklyOvernightStats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connString = Env.Variables.DatabaseURI;
                optionsBuilder.UseMySql(connString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.25-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<Crametorium>(entity =>
            {
                entity.ToTable("crametorium");

                entity.HasIndex(e => new { e.Date, e.Symbol }, "symbol date")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChangeClose).HasColumnName("change_close");

                entity.Property(e => e.ChangeHigh).HasColumnName("change_high");

                entity.Property(e => e.ChangeOpen).HasColumnName("change_open");

                entity.Property(e => e.Close).HasColumnName("close");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.High).HasColumnName("high");

                entity.Property(e => e.Open).HasColumnName("open");

                entity.Property(e => e.PickDate)
                    .HasColumnType("date")
                    .HasColumnName("pick_date");

                entity.Property(e => e.StartPrice).HasColumnName("start_price");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("symbol");
            });

            modelBuilder.Entity<GappersStat>(entity =>
            {
                entity.ToTable("gappers_stats");

                entity.HasIndex(e => new { e.Symbol, e.Date }, "symbol_date")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Close).HasColumnName("close");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.GapPct).HasColumnName("gap_pct");

                entity.Property(e => e.HalfHourPct).HasColumnName("half_hour_pct");

                entity.Property(e => e.IsGain).HasColumnName("is_gain");

                entity.Property(e => e.IsGapUp).HasColumnName("is_gap_up");

                entity.Property(e => e.Open).HasColumnName("open");

                entity.Property(e => e.PrevDayClose).HasColumnName("prev_day_close");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("symbol");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("groups");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Industry)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("industry");

                entity.Property(e => e.Sector)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("sector");
            });

            modelBuilder.Entity<GroupsDailyOverview>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("groups_daily_overview");

                entity.HasIndex(e => new { e.Date, e.Name }, "date_name_UN")
                    .IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.FwdPe).HasColumnName("FwdPE");

                entity.Property(e => e.Group)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("group");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Pe).HasColumnName("PE");

                entity.Property(e => e.Peg).HasColumnName("PEG");
            });

            modelBuilder.Entity<IntranightStat>(entity =>
            {
                entity.ToTable("intranight_stats");

                entity.HasIndex(e => new { e.Date, e.Symbol }, "date_symbol")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.DayVolume).HasColumnName("day_volume");

                entity.Property(e => e.EndPrice).HasColumnName("end_price");

                entity.Property(e => e.MaxPrice).HasColumnName("max_price");

                entity.Property(e => e.MaxPriceChange).HasColumnName("max_price_change");

                entity.Property(e => e.MaxPriceTime)
                    .HasColumnType("time")
                    .HasColumnName("max_price_time");

                entity.Property(e => e.MinPrice).HasColumnName("min_price");

                entity.Property(e => e.MinPriceChange).HasColumnName("min_price_change");

                entity.Property(e => e.MinPriceTime)
                    .HasColumnType("time")
                    .HasColumnName("min_price_time");

                entity.Property(e => e.StartPrice).HasColumnName("start_price");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("symbol");
            });

            modelBuilder.Entity<Price>(entity =>
            {
                entity.ToTable("prices");

                entity.HasIndex(e => new { e.Timestamp, e.Symbol }, "datetime_symbol")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Close).HasColumnName("close");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.High).HasColumnName("high");

                entity.Property(e => e.Interval)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("interval");

                entity.Property(e => e.Low).HasColumnName("low");

                entity.Property(e => e.Open).HasColumnName("open");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("symbol");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("timestamp");

                entity.Property(e => e.Volume).HasColumnName("volume");
            });

            modelBuilder.Entity<Prices1d>(entity =>
            {
                entity.ToTable("prices_1d");

                entity.HasIndex(e => new { e.Symbol, e.Timestamp }, "symbol_date")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Close).HasColumnName("close");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.High).HasColumnName("high");

                entity.Property(e => e.Interval)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("interval")
                    .HasDefaultValueSql("'1d'");

                entity.Property(e => e.Low).HasColumnName("low");

                entity.Property(e => e.Open).HasColumnName("open");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("symbol");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("date")
                    .HasColumnName("timestamp");

                entity.Property(e => e.Volume).HasColumnName("volume");
            });

            modelBuilder.Entity<Prices30m>(entity =>
            {
                entity.ToTable("prices_30m");

                entity.HasIndex(e => new { e.Timestamp, e.Symbol }, "datetime_symbol")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Close).HasColumnName("close");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.High).HasColumnName("high");

                entity.Property(e => e.Interval)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("interval")
                    .HasDefaultValueSql("'30m'");

                entity.Property(e => e.Low).HasColumnName("low");

                entity.Property(e => e.Open).HasColumnName("open");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("symbol");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("timestamp");

                entity.Property(e => e.Volume).HasColumnName("volume");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("schedules");

                entity.Property(e => e.EtfsAreDone)
                    .HasColumnName("etfs_are_done")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OvernightWeekEnd)
                    .HasColumnType("date")
                    .HasColumnName("overnight_week_end");

                entity.Property(e => e.OvernightWeekStart)
                    .HasColumnType("date")
                    .HasColumnName("overnight_week_start");

                entity.Property(e => e.StocksAreDone)
                    .HasColumnName("stocks_are_done")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.WeekNum).HasColumnName("week_num");
            });

            modelBuilder.Entity<Security>(entity =>
            {
                entity.ToTable("securities");

                entity.HasIndex(e => e.Symbol, "symbol")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Industry)
                    .HasColumnType("text")
                    .HasColumnName("industry");

                entity.Property(e => e.Sector)
                    .HasColumnType("text")
                    .HasColumnName("sector");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("symbol")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Type)
                    .HasColumnType("text")
                    .HasColumnName("type");
            });

            modelBuilder.Entity<WeeklyOvernightStat>(entity =>
            {
                entity.ToTable("weekly_overnight_stats");

                entity.HasIndex(e => new { e.Symbol, e.WeekNum, e.Strategy }, "symbol_week_num")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

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

                entity.Property(e => e.End)
                    .HasColumnType("date")
                    .HasColumnName("end");

                entity.Property(e => e.EndingBalance).HasColumnName("ending_balance");

                entity.Property(e => e.GrossLoss).HasColumnName("gross_loss");

                entity.Property(e => e.GrossProfit).HasColumnName("gross_profit");

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
                    .HasMaxLength(100)
                    .HasColumnName("security_type");

                entity.Property(e => e.SharpeRatio).HasColumnName("sharpe_ratio");

                entity.Property(e => e.SharpeRatioMax).HasColumnName("sharpe_ratio_max");

                entity.Property(e => e.SharpeRatioMin).HasColumnName("sharpe_ratio_min");

                entity.Property(e => e.SortinoRatio).HasColumnName("sortino_ratio");

                entity.Property(e => e.Start)
                    .HasColumnType("date")
                    .HasColumnName("start");

                entity.Property(e => e.StartPrice).HasColumnName("start_price");

                entity.Property(e => e.Strategy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("strategy");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("symbol");

                entity.Property(e => e.TotalNetProfit).HasColumnName("total_net_profit");

                entity.Property(e => e.WeekNum).HasColumnName("week_num");

                entity.Property(e => e.WorstDay).HasColumnName("worst_day");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
