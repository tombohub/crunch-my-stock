

public class Program
{
    static void Main(string[] args)
    {

        var ts = "2021-02-12 00:00:00";
        DateOnly d = DateOnly.ParseExact(ts, "yyyy-MM-dd HH:mm:ss");
        Console.WriteLine(d);
    }
}








