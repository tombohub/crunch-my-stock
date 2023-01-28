using System;

namespace Crunch.Core
{
    public record Security
    {
        public required Symbol Symbol { get; init; }
        public required Exchange Exchange { get; init; }
        public required SecurityType Type { get; init; }
        public required SecurityStatus Status { get; init; }
        public required DateOnly IpoDate { get; init; }
        public DateOnly? DelistingDate { get; init; }
    }
}