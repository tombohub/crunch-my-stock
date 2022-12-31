using Crunch.Domain;

namespace CrunchImport
{
    internal record SecurityDTO
    (
        string Symbol,
        SecurityType Type,
        string Exchange
    );
}