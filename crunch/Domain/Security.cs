namespace Crunch.Domain
{
    public class Security
    {
        public string Symbol { get; init; }
        public SecurityType Type { get; init; }

        public Security(string symbol, SecurityType type)
        {
            Symbol = symbol;
            Type = type;
        }
    }
}
