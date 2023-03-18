Error: 
    
    System.InvalidCastException: Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported. Note that it's not possible to mix DateTimes with different Kinds in an array/range.

Solved using **AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);**