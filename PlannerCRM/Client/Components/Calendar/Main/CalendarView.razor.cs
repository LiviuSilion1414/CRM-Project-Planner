using PlannerCRM.Client.Components.Calendar.Views;
using PlannerCRM.Client.Components.Panel;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Components.Calendar.Main;

public partial class CalendarView : ComponentBase
{
    [Inject] public IFetchService<ActivityDto> ActivityFetchService { get; set; }

    private ViewType CurrentView { get; set; } = ViewType.MonthView;
    private DateTime CurrentDate { get; set; } = DateTime.Today;

    private List<int?> CurrentMonthDays { get; set; } = new();
    private List<DateTime> CurrentWeekDays { get; set; } = new();

    private ComponentMetadata SelectedMetadata { get; set; } = new();

    private CascadingDataContainer<ActivityDto> DataContainer { get; set; } = new()
    {
        DataManager = new() 
        {
            MainItems = [],
            SelectedItems = [],
            SelectedProperties = [ "Name", "StartDate", "EndDate" ],
            NewItem = new()
            {
                WorkOrder = new() 
                { 
                    FirmClient = new()
                }
            },
            SelectedItem = new()
        }
    };  

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
        },
        {
            nameof(DataGridViewItemHandler<ActivityDto>), new ComponentMetadata
            {
                Name = nameof(DataGridViewItemHandler<ActivityDto>),
                ComponentType = typeof(DataGridViewItemHandler<ActivityDto>),
                Parameters = new Dictionary<string, object>()
            }
        },

    };

    protected override async Task OnInitializedAsync()
    {
        await LoadData(new PaginationHelper { Offset = 0, Limit = 100 });
        
        InitializeComponentParameters();
        GenerateCalendar();
        SelectedMetadata = ComponentsParameters[nameof(ViewType.MonthView)];
    }

    private async Task LoadData(PaginationHelper paginationHelper)
        => DataContainer.DataManager.MainItems =
                await ActivityFetchService.GetAll(ControllersNames.ACTIVITY, CrudApiManager.GET_WITH_PAGINATION, paginationHelper.Limit, paginationHelper.Offset);

    private async Task DeleteActivity(ActivityDto activity)
        => await ActivityFetchService.Delete(ControllersNames.ACTIVITY, CrudApiManager.DELETE, activity.Id);

    private async Task DeleteMultipleActivities(IEnumerable<ActivityDto> activitys)
    {
        foreach (var activity in activitys)
        {
            await DeleteActivity(activity);
        }
    }

    private void InitializeComponentParameters()
    {
        ComponentsParameters[nameof(MonthView)].Parameters = new Dictionary<string, object>
        {
            [nameof(MonthView.CurrentDate)] = CurrentDate,
            [nameof(MonthView.CurrentMonthDays)] = CurrentMonthDays,
            [nameof(MonthView.Activities)] = DataContainer.DataManager.MainItems,
            [nameof(MonthView.DataContainer)] = DataContainer,
            [nameof(MonthView.OnActivitySelected)] = EventCallback.Factory.Create<CascadingDataContainer<ActivityDto>>(this, GetChosenAction)
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

    private void GetChosenAction(CascadingDataContainer<ActivityDto> dataContainer)
        => DataContainer = dataContainer;

    private string GetDayClass(int? day)
    {
        if (!day.HasValue)
            return string.Empty;

        var currentDate = new DateTime(CurrentDate.Year, CurrentDate.Month, day.Value);
        return currentDate == DateTime.Today ? "today" : string.Empty;
    }
}
