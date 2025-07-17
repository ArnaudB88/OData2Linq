namespace OData2Linq.Settings
{
    [Flags]
    public enum AllowedFunctions
    {
        None = 0,
        StartsWith = 1 << 0,
        EndsWith = 1 << 1,
        Contains = 1 << 2,
        Length = 1 << 3,
        IndexOf = 1 << 4,
        Concat = 1 << 5,
        Substring = 1 << 6,
        ToLower = 1 << 7,
        ToUpper = 1 << 8,
        Trim = 1 << 9,
        Cast = 1 << 10,
        Year = 1 << 11,
        Date = 1 << 12,
        Month = 1 << 13,
        Time = 1 << 14,
        Day = 1 << 15,
        Hour = 1 << 16,
        Minute = 1 << 17,
        Second = 1 << 18,
        FractionalSeconds = 19 << 0,
        Round = 1 << 20,
        Floor = 1 << 21,
        Ceiling = 1 << 22,
        IsOf = 1 << 23,
        Any = 1 << 24,
        All = 1 << 25,
        MatchesPattern = 1 << 26,
        AllStringFunctions = MatchesPattern | Trim | ToUpper | ToLower | Substring | Concat | IndexOf | Length | Contains | EndsWith | StartsWith,
        AllDateTimeFunctions = FractionalSeconds | Second | Minute | Hour | Day | Time | Month | Date | Year,
        AllMathFunctions = Ceiling | Floor | Round,
        AllFunctions = AllMathFunctions | AllDateTimeFunctions | AllStringFunctions | All | Any | IsOf | Cast
    }
}