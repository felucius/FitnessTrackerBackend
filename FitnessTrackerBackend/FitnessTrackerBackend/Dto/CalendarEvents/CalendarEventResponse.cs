namespace FitnessTrackerBackend.Dto.CalendarEvents
{
    public sealed record CalendarEventResponse (
        
        Guid Id,
        Guid WorkoutPlanId,
        string Title,
        DateTimeOffset Start,
        DateTimeOffset End,
        bool AllDay

        // Additions will be coming in later
    );
}
