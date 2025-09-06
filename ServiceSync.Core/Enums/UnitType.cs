namespace ServiceSync.Core.Enums;

public enum UnitType
{
    Fixed,      // 0: A single, fixed-price item (e.g., a service fee)
    PerHour,    // 1: Priced per hour of labor
    PerItem     // 2: Priced per individual unit (e.g., materials)
}