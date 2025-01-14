using PlannerCRM.Client.Components.Calendar.Views;
using System.Net.Http.Json;

namespace PlannerCRM.Client.Components.Calendar.Main;

public partial class CalendarView : ComponentBase
{
    [Inject] public HttpClient Http { get; set; }

    private ViewType CurrentView { get; set; } = ViewType.MonthView;
    private DateTime CurrentDate { get; set; } = DateTime.Today;

    private List<int?> CurrentMonthDays { get; set; } = new List<int?>();
    private List<DateTime> CurrentWeekDays { get; set; } = new List<DateTime>();

    private List<ActivityDto> Activities { get; set; } = new();
    private ComponentMetadata SelectedMetadata { get; set; } = new();

    private Dictionary<string, ComponentMetadata> ComponentsParameters { get; set; } = new()
    {
        {
            nameof(MonthView), new ComponentMetadata
            {
                Name = nameof(MonthView),
                ComponentType = typeof(MonthView),
                Parameters = new Dictionary<string, object>()
            }
        },
        {
            nameof(WeekView), new ComponentMetadata
            {
                Name = nameof(WeekView),
                ComponentType = typeof(WeekView),
                Parameters = new Dictionary<string, object>()
            }
        },
        {
            nameof(DayView), new ComponentMetadata
            {
                Name = nameof(DayView),
                ComponentType = typeof(DayView),
                Parameters = new Dictionary<string, object>()
            }
        },
        {
            nameof(YearView), new ComponentMetadata
            {
                Name = nameof(YearView),
                ComponentType = typeof(YearView),
                Parameters = new Dictionary<string, object>()
            }
        }
    };

    protected override async Task OnInitializedAsync()
    {
        Activities = await Http.GetFromJsonAsync<List<ActivityDto>>($"api/activity/getWithPagination/{50}/{0}");
        InitializeComponentParameters();
        GenerateCalendar();
        SelectedMetadata = ComponentsParameters[nameof(ViewType.MonthView)];
    }

    private void InitializeComponentParameters()
    {
        ComponentsParameters[nameof(MonthView)].Parameters = new Dictionary<string, object>
        {
            [nameof(MonthView.CurrentDate)] = CurrentDate,
            [nameof(MonthView.CurrentMonthDays)] = CurrentMonthDays,
            [nameof(MonthView.Activities)] = Activities
        };

        ComponentsParameters[nameof(WeekView)].Parameters = new Dictionary<string, object>
        {
            [nameof(WeekView.CurrentDate)] = CurrentDate,
            [nameof(WeekView.CurrentWeekDays)] = CurrentWeekDays
        };

        ComponentsParameters[nameof(DayView)].Parameters = new Dictionary<string, object>
        {
            [nameof(DayView.CurrentDate)] = CurrentDate
        };

        ComponentsParameters[nameof(YearView)].Parameters = new Dictionary<string, object>
        {
            [nameof(YearView.CurrentDate)] = CurrentDate
        };
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
        SelectedMetadata = ComponentsParameters[view.ToString()];
        GenerateCalendar();
    }

    private void GoToToday()
    {
        CurrentDate = DateTime.Today;
        GenerateCalendar();
    }

    private void PreviousPeriod()
    {
        if (CurrentView == ViewType.MonthView)
            CurrentDate = CurrentDate.AddMonths(-1);
        else if (CurrentView == ViewType.WeekView)
            CurrentDate = CurrentDate.AddDays(-7);
        else if (CurrentView == ViewType.DayView)
            CurrentDate = CurrentDate.AddDays(-1);
        else if (CurrentView == ViewType.YearView)
            CurrentDate = CurrentDate.AddYears(-1);

        GenerateCalendar();
    }

    private void NextPeriod()
    {
        if (CurrentView == ViewType.MonthView)
            CurrentDate = CurrentDate.AddMonths(1);
        else if (CurrentView == ViewType.WeekView)
            CurrentDate = CurrentDate.AddDays(7);
        else if (CurrentView == ViewType.DayView)
            CurrentDate = CurrentDate.AddDays(1);
        else if (CurrentView == ViewType.YearView)
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
