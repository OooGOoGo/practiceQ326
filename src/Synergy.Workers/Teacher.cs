namespace Synergy.Workers;

public class Teacher : Worker
{
    private string _department;

    public Teacher()
    {
        _department = "Не указана";
    }

    public Teacher(string fullName, string position, decimal salary, int hireYear, string department)
        : base(fullName, position, salary, hireYear)
    {
        _department = Validation.RequireText(department, "Кафедра");
    }

    public string Department
    {
        get => _department;
        set => _department = Validation.RequireText(value, "Кафедра");
    }

    public override string Describe() => $"{base.Describe()} | кафедра: {_department}";
}
