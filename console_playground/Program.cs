namespace console_playground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var k = new TimePeriod(new DateOnly(2022, 3, 6), new DateOnly(2022, 3, 5));
            Console.WriteLine(k);
        }
    }

    internal record TimePeriod
    {
        public DateOnly Start { get; init; }
        public DateOnly End { get; }

        public TimePeriod(DateOnly start, DateOnly end)
        {
            if (start > end)
            {
                throw new ArgumentException("Start date cannot be later than end date");
            }
        }
    }
}