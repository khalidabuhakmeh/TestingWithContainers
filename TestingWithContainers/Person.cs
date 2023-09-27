namespace TestingWithContainers;

// ReSharper disable once MemberCanBePrivate.Global
public class Person(string name = "")
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int Id { get; set; }
    // ReSharper disable once UnusedMember.Global
    public string Name { get; set; } = name;
}; 