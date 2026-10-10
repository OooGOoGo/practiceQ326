using Synergy.Workers;

namespace Synergy.Workers.Tests;

public class WorkerTests
{
    private static readonly int ThisYear = DateTime.Today.Year;

    [Fact]
    public void DefaultConstructor_SetsPlaceholders()
    {
        var w = new Worker();
        Assert.Equal("Не указано", w.FullName);
        Assert.Equal("Не указана", w.Position);
        Assert.Equal(0m, w.Salary);
        Assert.Equal(ThisYear, w.HireYear);
    }

    [Fact]
    public void ShortConstructor_SetsNameAndPosition()
    {
        var w = new Worker("Петров П.П.", "Методист");
        Assert.Equal("Петров П.П.", w.FullName);
        Assert.Equal("Методист", w.Position);
        Assert.Equal(0m, w.Salary);
    }

    [Fact]
    public void FullConstructor_TrimsAndStoresAllFields()
    {
        var w = new Worker("  Иванов И.И. ", "Доцент", 85000m, 2015);
        Assert.Equal("Иванов И.И.", w.FullName);
        Assert.Equal("Доцент", w.Position);
        Assert.Equal(85000m, w.Salary);
        Assert.Equal(2015, w.HireYear);
    }

    [Fact]
    public void CopyConstructor_CreatesIndependentCopy()
    {
        var original = new Worker("Иванов И.И.", "Доцент", 85000m, 2015);
        var copy = new Worker(original);
        copy.Salary = 1m;
        Assert.Equal(85000m, original.Salary);
        Assert.Equal(original.FullName, copy.FullName);
    }

    [Theory]
    [InlineData("", "Доцент")]
    [InlineData("Иванов И.И.", "   ")]
    public void Constructor_RejectsEmptyText(string name, string position)
    {
        Assert.Throws<ArgumentException>(() => new Worker(name, position, 1m, 2015));
    }

    [Theory]
    [InlineData(-1, 2015)]
    [InlineData(1000, 1800)]
    public void Constructor_RejectsBadNumbers(decimal salary, int year)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Worker("Иванов И.И.", "Доцент", salary, year));
    }

    [Fact]
    public void HireYear_InFuture_IsRejected()
    {
        var w = new Worker("Иванов И.И.", "Доцент", 1m, 2015);
        Assert.Throws<ArgumentOutOfRangeException>(() => w.HireYear = ThisYear + 1);
        Assert.Equal(2015, w.HireYear);
    }

    [Fact]
    public void Setters_KeepOldValueOnError()
    {
        var w = new Worker("Иванов И.И.", "Доцент", 85000m, 2015);
        Assert.Throws<ArgumentOutOfRangeException>(() => w.Salary = -5m);
        Assert.Throws<ArgumentException>(() => w.FullName = "");
        Assert.Equal(85000m, w.Salary);
        Assert.Equal("Иванов И.И.", w.FullName);
    }

    [Fact]
    public void ChangePosition_IsAllOrNothing()
    {
        var w = new Worker("Иванов И.И.", "Доцент", 85000m, 2015);
        w.ChangePosition("Профессор", 120000m);
        Assert.Equal("Профессор", w.Position);
        Assert.Equal(120000m, w.Salary);

        Assert.Throws<ArgumentOutOfRangeException>(() => w.ChangePosition("Декан", -1m));
        Assert.Equal("Профессор", w.Position);
    }

    [Fact]
    public void IndexSalary_AppliesPercentAndRounds()
    {
        var w = new Worker("Иванов И.И.", "Доцент", 1000m, 2015);
        w.IndexSalary(7.5m);
        Assert.Equal(1075m, w.Salary);
        Assert.Throws<ArgumentOutOfRangeException>(() => w.IndexSalary(-100m));
    }

    [Theory]
    [InlineData(2026, 11)]
    [InlineData(2015, 0)]
    [InlineData(2010, 0)]
    public void GetExperience_CountsFullYears(int currentYear, int expected)
    {
        var w = new Worker("Иванов И.И.", "Доцент", 1m, 2015);
        Assert.Equal(expected, w.GetExperience(currentYear));
    }

    [Fact]
    public void HasExperienceOver_IsStrict()
    {
        var w = new Worker("Иванов И.И.", "Доцент", 1m, 2015);
        Assert.True(w.HasExperienceOver(10, 2026));
        Assert.False(w.HasExperienceOver(11, 2026));
    }

    [Fact]
    public void Describe_IsPolymorphic()
    {
        Person worker = new Worker("Иванов И.И.", "Доцент", 85000m, 2015);
        Person teacher = new Teacher("Смирнова А.В.", "Профессор", 120000m, 2005, "Кафедра информатики");

        Assert.Equal("Иванов И.И. | Доцент | 85 000,00 руб. | с 2015 г.",
            worker.Describe().Replace(' ', ' ').Replace(' ', ' '));
        Assert.EndsWith("| кафедра: Кафедра информатики", teacher.Describe());
        Assert.StartsWith("Смирнова А.В. | Профессор", teacher.Describe());
    }

    [Fact]
    public void Display_WritesDescriptionLine()
    {
        var w = new Worker("Иванов И.И.", "Доцент", 1m, 2015);
        var output = new StringWriter();
        w.Display(output);
        Assert.Equal(w.Describe() + Environment.NewLine, output.ToString());
    }
}
