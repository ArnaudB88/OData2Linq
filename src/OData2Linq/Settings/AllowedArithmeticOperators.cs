namespace OData2Linq.Settings
{
    [Flags]
    public enum AllowedArithmeticOperators
    {
        None = 0,
        Add = 1,
        Subtract = 2,
        Multiply = 4,
        Divide = 8,
        Modulo = 16,
        All = Modulo | Divide | Multiply | Subtract | Add
    }
}