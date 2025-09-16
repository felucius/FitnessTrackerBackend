using FitnessTrackerBackend.Dto.CalendarEvents;
using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.Mappings
{
    public static class CalendarEventMappings
    {
        public static CalendarEventResponse ToResponse(this CalendarEvent calendarEvent) =>
            new CalendarEventResponse(
                calendarEvent.Id,
                calendarEvent.Title,
                calendarEvent.WorkoutPlanId,
                calendarEvent.Start,
                calendarEvent.End,
                calendarEvent.AllDay
            );

        public static CalendarEvent ToModel(this CreateCalendarEventRequest request) =>
            new CalendarEvent
            {
                Title = request.Title,
                WorkoutPlanId = request.WorkoutPlanId,
                Start = request.Start,
                End = request.End,
                AllDay = request.AllDay
            };
    }
}
