Error: 
    
    System.InvalidCastException: Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported. Note that it's not possible to mix DateTimes with different Kinds in an array/range.

Solved using **AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);**

Cannot pass integer as parameter in blazor pages 
Error:  Unhandled exception rendering component: Unable to set property 'paramId' on object of type 'PlannerCRM.Client.Pages.AccountManager.AccountManagerEditUserForm'. The error was: Specified cast is not valid

Solved by changing the type of page parameter from int to string because Razor Pages don't accept int as page parameters

This works: [Parameter] public string message { get; set; } 
This does NOT works: [Parameter] public int message { get; set; } 
