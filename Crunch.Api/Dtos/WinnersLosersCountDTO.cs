namespace Crunch.Api.Dtos
{
    public record WinnersLosersCountDTO
    {
        public required DateOnly Date { get; init; }
        public required int WinnersCount { get; init; }
        public required int LosersCount { get; init; }
    }
}
