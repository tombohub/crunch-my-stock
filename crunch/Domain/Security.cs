namespace Crunch.Domain
{
    public record Security
    {
        public Symbol Symbol { get; init; }
        public Exchange Exchange { get; init; }
        public SecurityType Type { get; init; }
        public bool IsTradable { get; init; }
    }
}