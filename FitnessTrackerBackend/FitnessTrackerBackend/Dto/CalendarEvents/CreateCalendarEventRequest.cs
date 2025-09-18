namespace FitnessTrackerBackend.Dto.CalendarEvents
{
    public sealed record CreateCalendarEventRequest
    (
        string Title,
        Guid WorkoutPlanId,
        DateTimeOffset Start,
        DateTimeOffset End,
        bool AllDay
    );
}
