using PlannerCRM.Client.Components.Calendar.Views;

namespace PlannerCRM.Client.Components.Calendar.Main;

public partial class CalendarView : ComponentBase
{
    private static ViewType CurrentView { get; set; } = ViewType.Month;
    private static DateTime CurrentDate { get; set; } = DateTime.Today;

    private static List<int?> CurrentMonthDays { get; set; } = new List<int?>();
    private static List<DateTime> CurrentWeekDays { get; set; } = new List<DateTime>();

    private static readonly List<CalendarEvent> Events = new List<CalendarEvent>
    {
        new CalendarEvent { Title = "Task 1", Color = "orange", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(2) },
        new CalendarEvent { Title = "Task 2", Color = "purple", StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now },
        new CalendarEvent { Title = "Task 3", Color = "blue", StartDate = DateTime.Now.AddDays(-7), EndDate = DateTime.Now.AddDays(-2) },
        new CalendarEvent { Title = "Task 4", Color = "green", StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now.AddDays(-7) },
        new CalendarEvent { Title = "Task 5", Color = "red", StartDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(9) }
    };

    private static readonly Dictionary<string, Type> ViewTypes = new Dictionary<string, Type>
    {
        { nameof(MonthView), typeof(MonthView) },
        { nameof(WeekView), typeof(WeekView) },
        { nameof(DayView), typeof(DayView) },
        { nameof(YearView), typeof(YearView) }
    };

    private static ComponentMetadata _selectedMetadata = null;
    private static readonly Dictionary<string, ComponentMetadata> ComponentsParameters = new Dictionary<string, ComponentMetadata>
    {
        {
            nameof(MonthView), new ComponentMetadata
            {
                Name = nameof(MonthView),
                ComponentType = typeof(MonthView),
                Parameters = new Dictionary<string, object>
                {
                    [nameof(MonthView.CurrentDate)] = CurrentDate,
                    [nameof(MonthView.CurrentMonthDays)] = CurrentMonthDays,
                    [nameof(MonthView.Events)] = Events
                }
            }
        },
        {
            nameof(WeekView), new ComponentMetadata
            {
                Name = nameof(WeekView),
                ComponentType = typeof(WeekView),
                Parameters = new Dictionary<string, object>
                {
                    [nameof(WeekView.CurrentDate)] = CurrentDate,
                    [nameof(WeekView.CurrentWeekDays)] = CurrentWeekDays
                }
            }
        },
        {
            nameof(DayView), new ComponentMetadata
            {
                Name = nameof(DayView),
                ComponentType = typeof(DayView),
                Parameters = new Dictionary<string, object>
                {
                    [nameof(DayView.CurrentDate)] = CurrentDate
                }
            }
        },
        {
            nameof(YearView), new ComponentMetadata
            {
                Name = nameof(YearView),
                ComponentType = typeof(YearView),
                Parameters = new Dictionary<string, object>
                {
                    [nameof(YearView.CurrentDate)] = CurrentDate
                }
            }
        }
    };

    protected override void OnInitialized()
    {
        GenerateCalendar();
    }

    private class ComponentMetadata
    {
        public Type ComponentType { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }

    private static void OnDropDownChange(ChangeEventArgs e)
    {
        if (ComponentsParameters.TryGetValue(e.Value?.ToString() ?? string.Empty, out var metadata))
        {
            _selectedMetadata = metadata;
        } else
        {
            Console.WriteLine($"Chiave {e.Value} non trovata nel dizionario.");
        }
    }

    private static void GenerateCalendar()
    {
        CurrentMonthDays.Clear();

        var firstDayOfMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);

        // Set Monday as first day of the week
        int dayOfWeek = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;

        for (int i = 0 ; i < dayOfWeek ; i++)
        {
            CurrentMonthDays.Add(null);
        }

        for (int i = 1 ; i <= daysInMonth ; i++)
        {
            CurrentMonthDays.Add(i);
        }

        GenerateWeekDays();
    }

    private static void GenerateWeekDays()
    {
        CurrentWeekDays.Clear();

        // Set Monday as first day of the week
        var startOfWeek = CurrentDate.AddDays(-(((int)CurrentDate.DayOfWeek + 6) % 7));
        for (int i = 0 ; i < 7 ; i++)
        {
            CurrentWeekDays.Add(startOfWeek.AddDays(i));
        }
    }

    private static void SetView(ViewType view)
    {
        CurrentView = view;
        GenerateCalendar();
    }

    private static void GoToToday()
    {
        CurrentDate = DateTime.Today;
        GenerateCalendar();
    }

    private static void PreviousPeriod()
    {
        if (CurrentView == ViewType.Month)
            CurrentDate = CurrentDate.AddMonths(-1);
        else if (CurrentView == ViewType.Week)
            CurrentDate = CurrentDate.AddDays(-7);
        else if (CurrentView == ViewType.Day)
            CurrentDate = CurrentDate.AddDays(-1);
        else if (CurrentView == ViewType.Year)
            CurrentDate = CurrentDate.AddYears(-1);

        GenerateCalendar();
    }

    private static void NextPeriod()
    {
        if (CurrentView == ViewType.Month)
            CurrentDate = CurrentDate.AddMonths(1);
        else if (CurrentView == ViewType.Week)
            CurrentDate = CurrentDate.AddDays(7);
        else if (CurrentView == ViewType.Day)
            CurrentDate = CurrentDate.AddDays(1);
        else if (CurrentView == ViewType.Year)
            CurrentDate = CurrentDate.AddYears(1);

        GenerateCalendar();
    }

    private static string GetDayClass(int? day)
    {
        if (!day.HasValue)
            return string.Empty;

        var currentDate = new DateTime(CurrentDate.Year, CurrentDate.Month, day.Value);
        return currentDate == DateTime.Today ? "today" : string.Empty;
    }
}
