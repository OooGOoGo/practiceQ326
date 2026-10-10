using System.Globalization;

namespace Synergy.Workers;

public static class WorkerConsole
{
    public static Worker? ReadWorker(TextReader input, TextWriter output, int currentYear)
    {
        var name = ReadText(input, output, "  Фамилия и инициалы: ");
        if (name is null) return null;
        var position = ReadText(input, output, "  Должность: ");
        if (position is null) return null;
        var salary = ReadNumber(input, output, "  Зарплата, руб.: ", 0m, 100_000_000m);
        if (salary is null) return null;
        var year = ReadInt(input, output, "  Год поступления на работу: ", Validation.MinHireYear, currentYear);
        if (year is null) return null;
        var answer = ReadText(input, output, "  Преподаватель (д/н): ");
        if (answer is null) return null;
        if (answer.StartsWith("д", StringComparison.OrdinalIgnoreCase) ||
            answer.StartsWith("y", StringComparison.OrdinalIgnoreCase))
        {
            var department = ReadText(input, output, "  Кафедра: ");
            if (department is null) return null;
            return new Teacher(name, position, salary.Value, year.Value, department);
        }
        return new Worker(name, position, salary.Value, year.Value);
    }

    public static List<Worker> FindExperienced(IEnumerable<Worker> workers, int years, int currentYear) =>
        workers.Where(w => w.HasExperienceOver(years, currentYear)).ToList();

    public static string? ReadText(TextReader input, TextWriter output, string prompt)
    {
        while (true)
        {
            output.Write(prompt);
            var line = input.ReadLine();
            if (line is null) return null;
            line = line.Trim();
            if (line.Length > 0) return line;
            output.WriteLine("  Значение не может быть пустым, повторите ввод.");
        }
    }

    public static decimal? ReadNumber(TextReader input, TextWriter output, string prompt, decimal min, decimal max)
    {
        while (true)
        {
            output.Write(prompt);
            var line = input.ReadLine();
            if (line is null) return null;
            var normalized = line.Trim().Replace(" ", "").Replace(',', '.');
            if (decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out var value)
                && value >= min && value <= max)
                return value;
            output.WriteLine($"  Введите число от {min} до {max}.");
        }
    }

    public static int? ReadInt(TextReader input, TextWriter output, string prompt, int min, int max)
    {
        while (true)
        {
            output.Write(prompt);
            var line = input.ReadLine();
            if (line is null) return null;
            if (int.TryParse(line.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
                && value >= min && value <= max)
                return value;
            output.WriteLine($"  Введите целое число от {min} до {max}.");
        }
    }

    public static string YearsWord(int n)
    {
        int mod100 = n % 100, mod10 = n % 10;
        if (mod100 is >= 11 and <= 14) return "лет";
        return mod10 switch { 1 => "год", >= 2 and <= 4 => "года", _ => "лет" };
    }

    public static string YearsGenitive(int n) => n % 10 == 1 && n % 100 != 11 ? "года" : "лет";
}
