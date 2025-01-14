namespace PlannerCRM.Client.Components.Calendar.Main;

public partial class CalendarView : ComponentBase
{
    private ViewType CurrentView { get; set; } = ViewType.Month;
    private DateTime CurrentDate { get; set; } = DateTime.Today;

    private List<int?> CurrentMonthDays { get; set; } = [];
    private List<DateTime> CurrentWeekDays { get; set; } = [];

    private readonly List<CalendarEvent> _events =
    [
        new() { Title = "Task 1", Color="orange", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(2) },
        new() { Title = "Task 2", Color="purple", StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now },
        new() { Title = "Task 3", Color="blue", StartDate = DateTime.Now.AddDays(-7), EndDate = DateTime.Now.AddDays(-2) },
        new() { Title = "Task 4", Color="green", StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now.AddDays(-7) },
        new() { Title = "Task 5", Color="red", StartDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(9) }
    ];

    protected override void OnInitialized()
    {
        GenerateCalendar();
        Console.WriteLine(DateTime.Now);
    }


    private void GenerateCalendar()
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


    private void GenerateWeekDays()
    {
        CurrentWeekDays.Clear();

        // Set Monday as first day of the week
        var startOfWeek = CurrentDate.AddDays(-(((int)CurrentDate.DayOfWeek + 6) % 7));
        for (int i = 0 ; i < 7 ; i++)
        {
            CurrentWeekDays.Add(startOfWeek.AddDays(i));
        }
    }


    private void SetView(ViewType view)
    {
        CurrentView = view;
        GenerateCalendar();
    }

    private void GoToToday()
    {
        CurrentDate = DateTime.Today;
        GenerateCalendar();
    }

    private void PreviousPeriod()
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

    private void NextPeriod()
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

    private string GetDayClass(int? day)
    {
        if (!day.HasValue)
            return string.Empty;

        var currentDate = new DateTime(CurrentDate.Year, CurrentDate.Month, day.Value);
        return currentDate == DateTime.Today ? "today" : string.Empty;
    }
}
