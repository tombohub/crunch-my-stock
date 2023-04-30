public class Example
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Example(int id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class MappedExample
{
    public int Id { get; set; }
    public string NewName { get; set; }

    public MappedExample(int id, string newName)
    {
        Id = id;
        NewName = newName;
    }
}

public class Program
{
    static void Main(string[] args)
    {
        List<Example> examples = new List<Example>();
        for (int i = 0; i < 1000; i++)
        {
            examples.Add(new Example(i, "Example " + i));
        }

        List<MappedExample> mappedExamples = new List<MappedExample>();
        foreach (var example in examples)
        {
            Console.WriteLine($"mapping {example.Id}");
            mappedExamples.Add(new MappedExample(example.Id, "Mapped " + example.Name));
        }

        // Do something with the mappedExamples list
    }
}
