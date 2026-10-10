using Synergy.Workers;

namespace Synergy.Workers.Tests;

public class WorkerConsoleTests
{
    private static List<Worker> SampleStaff() =>
    [
        new Worker("Иванов И.И.", "Доцент", 85000m, 2015),
        new Worker("Петров П.П.", "Методист", 60000m, 2022),
        new Teacher("Смирнова А.В.", "Профессор", 120000m, 2005, "Кафедра информатики"),
    ];

    [Fact]
    public void FindExperienced_ReturnsOnlyLongerServiceInOrder()
    {
        var found = WorkerConsole.FindExperienced(SampleStaff(), 5, 2026);
        Assert.Equal(new[] { "Иванов И.И.", "Смирнова А.В." }, found.Select(w => w.FullName));
    }

    [Fact]
    public void FindExperienced_ReturnsEmptyWhenNobodyMatches()
    {
        Assert.Empty(WorkerConsole.FindExperienced(SampleStaff(), 30, 2026));
        Assert.Empty(WorkerConsole.FindExperienced(new List<Worker>(), 0, 2026));
    }

    [Fact]
    public void ReadWorker_RepeatsInvalidInput()
    {
        var input = new StringReader("Кузнецов К.К.\n\nМетодист\n-100\nabc\n55 000,5\n1700\n2019.5\n2019\nн\n");
        var output = new StringWriter();

        var w = WorkerConsole.ReadWorker(input, output, 2026);

        Assert.NotNull(w);
        Assert.IsNotType<Teacher>(w);
        Assert.Equal("Кузнецов К.К.", w.FullName);
        Assert.Equal("Методист", w.Position);
        Assert.Equal(55000.5m, w.Salary);
        Assert.Equal(2019, w.HireYear);
        Assert.Contains("не может быть пустым", output.ToString());
    }

    [Fact]
    public void ReadWorker_CreatesTeacherWhenAnswerIsYes()
    {
        var input = new StringReader("Смирнова А.В.\nПрофессор\n120000\n2005\nд\nКафедра информатики\n");
        var w = WorkerConsole.ReadWorker(input, new StringWriter(), 2026);

        var teacher = Assert.IsType<Teacher>(w);
        Assert.Equal("Кафедра информатики", teacher.Department);
    }

    [Fact]
    public void ReadWorker_ReturnsNullWhenInputEnds()
    {
        var input = new StringReader("Новиков Н.Н.\nТехник\n");
        Assert.Null(WorkerConsole.ReadWorker(input, new StringWriter(), 2026));
    }

    [Theory]
    [InlineData(1, "год")]
    [InlineData(3, "года")]
    [InlineData(5, "лет")]
    [InlineData(11, "лет")]
    [InlineData(21, "год")]
    public void YearsWord_AgreesWithNumber(int n, string expected)
    {
        Assert.Equal(expected, WorkerConsole.YearsWord(n));
    }

    [Theory]
    [InlineData(1, "года")]
    [InlineData(5, "лет")]
    [InlineData(11, "лет")]
    public void YearsGenitive_AgreesWithNumber(int n, string expected)
    {
        Assert.Equal(expected, WorkerConsole.YearsGenitive(n));
    }
}
