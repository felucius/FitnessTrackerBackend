namespace FitnessTrackerBackend.Dto.CalendarEvents
{
    public sealed record CalendarEventResponse (
        
        Guid Id,
        string Title,
        Guid WorkoutPlanId,
        DateTimeOffset Start,
        DateTimeOffset End,
        bool AllDay

        // Additions will be coming in later
    );
}
