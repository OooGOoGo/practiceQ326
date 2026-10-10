using System.Text;
using Synergy.Workers;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

const string Organization = "Университет «Синергия»";
int currentYear = DateTime.Today.Year;
TextReader input = Console.In;
TextWriter output = Console.Out;

output.WriteLine($"Учёт работников: {Organization}");
output.WriteLine();

var count = WorkerConsole.ReadInt(input, output, "Сколько работников ввести? ", 1, 1000);
if (count is null) return;

var staff = new List<Worker>();
for (int i = 1; i <= count; i++)
{
    output.WriteLine();
    output.WriteLine($"Работник {i} из {count}:");
    var worker = WorkerConsole.ReadWorker(input, output, currentYear);
    if (worker is null)
    {
        output.WriteLine("Ввод прерван.");
        break;
    }
    staff.Add(worker);
}

output.WriteLine();
output.WriteLine("Список работников:");
foreach (var w in staff)
{
    int exp = w.GetExperience(currentYear);
    output.WriteLine($"  {w.Describe()} | стаж {exp} {WorkerConsole.YearsWord(exp)}");
}

output.WriteLine();
var years = WorkerConsole.ReadInt(input, output, "Показать работников со стажем больше (лет): ", 0, 100);
if (years is null) return;
int n = years.Value;

var found = WorkerConsole.FindExperienced(staff, n, currentYear);
if (found.Count == 0)
{
    output.WriteLine($"Работников со стажем больше {n} {WorkerConsole.YearsGenitive(n)} нет.");
}
else
{
    output.WriteLine($"Работники со стажем больше {n} {WorkerConsole.YearsGenitive(n)}:");
    foreach (var w in found)
        output.WriteLine($"  {w.FullName}");
}
