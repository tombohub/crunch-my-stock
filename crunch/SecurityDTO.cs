using System;
using Crunch.Core;

namespace Crunch
{
    internal record SecurityDTO
    {
        public required string Symbol { get; init; }
        public required SecurityStatus Status { get; init; }
        public required Exchange Exchange { get; init; }
        public required SecurityType Type { get; init; }
        public required DateOnly IpoDate { get; init; }
        public DateOnly? DelistingDate { get; init; }
    }
}