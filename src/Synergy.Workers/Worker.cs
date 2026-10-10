using System.Globalization;

namespace Synergy.Workers;

public class Worker : Person
{
    private static readonly CultureInfo Ru = CultureInfo.GetCultureInfo("ru-RU");

    private string _position;
    private decimal _salary;
    private int _hireYear;

    public Worker()
    {
        _position = "Не указана";
        _salary = 0m;
        _hireYear = DateTime.Today.Year;
    }

    public Worker(string fullName, string position)
        : base(fullName)
    {
        _position = Validation.RequireText(position, "Должность");
        _salary = 0m;
        _hireYear = DateTime.Today.Year;
    }

    public Worker(string fullName, string position, decimal salary, int hireYear)
        : this(fullName, position)
    {
        Salary = salary;
        HireYear = hireYear;
    }

    public Worker(Worker other)
        : this(other.FullName, other.Position, other.Salary, other.HireYear)
    {
    }

    ~Worker()
    {
        System.Diagnostics.Debug.WriteLine($"Worker уничтожен: {FullName}");
    }

    public string Position
    {
        get => _position;
        set => _position = Validation.RequireText(value, "Должность");
    }

    public decimal Salary
    {
        get => _salary;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Зарплата не может быть отрицательной.");
            _salary = value;
        }
    }

    public int HireYear
    {
        get => _hireYear;
        set
        {
            if (value < Validation.MinHireYear || value > DateTime.Today.Year)
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"Год поступления должен быть от {Validation.MinHireYear} до {DateTime.Today.Year}.");
            _hireYear = value;
        }
    }

    public void ChangePosition(string newPosition, decimal newSalary)
    {
        var position = Validation.RequireText(newPosition, "Должность");
        if (newSalary < 0)
            throw new ArgumentOutOfRangeException(nameof(newSalary), "Зарплата не может быть отрицательной.");
        _position = position;
        _salary = newSalary;
    }

    public void IndexSalary(decimal percent)
    {
        if (percent <= -100)
            throw new ArgumentOutOfRangeException(nameof(percent), "Процент должен быть больше −100.");
        _salary = Math.Round(_salary * (100 + percent) / 100, 2);
    }

    public int GetExperience(int currentYear) => Math.Max(0, currentYear - _hireYear);

    public bool HasExperienceOver(int years, int currentYear) => GetExperience(currentYear) > years;

    public override string Describe() =>
        string.Format(Ru, "{0} | {1} | {2:N2} руб. | с {3} г.", FullName, _position, _salary, _hireYear);
}
