namespace Synergy.Workers;

public class Person
{
    private string _fullName;

    public Person()
    {
        _fullName = "Не указано";
    }

    public Person(string fullName)
    {
        _fullName = Validation.RequireText(fullName, "ФИО");
    }

    public string FullName
    {
        get => _fullName;
        set => _fullName = Validation.RequireText(value, "ФИО");
    }

    public virtual string Describe() => _fullName;

    public void Display(TextWriter? output = null) => (output ?? Console.Out).WriteLine(Describe());

    public override string ToString() => Describe();
}
