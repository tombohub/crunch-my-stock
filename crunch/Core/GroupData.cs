using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace Crunch.Core
{
    /// <summary>
    /// Group (sector, industry) performance overview entity
    /// </summary>
    class GroupData
    {

        public string Name { get; set; }
        public ushort Stocks { get; set; }
        public ulong MarketCap { get; set; }
        public string Dividend { get; set; }
        public string PE { get; set; }
        public string FwdPE { get; set; }
        public string PEG { get; set; }
        public string FloatShort { get; set; }
        public decimal Change { get; set; }
        public decimal Volume { get; set; }
        public string group { get; set; }
        public DateTime date { get; set; }
    }
}
