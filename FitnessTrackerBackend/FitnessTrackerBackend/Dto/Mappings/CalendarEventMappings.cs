using FitnessTrackerBackend.Dto.CalendarEvents;
using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.Mappings
{
    public static class CalendarEventMappings
    {
        public static CalendarEventResponse ToResponse(this CalendarEvent calendarEvent)
        {
            var response = new CalendarEventResponse(
                calendarEvent.Id,
                calendarEvent.WorkoutPlanId,
                calendarEvent.Title,
                calendarEvent.Start,
                calendarEvent.End,
                calendarEvent.AllDay
            );

            return response;
        }

        public static CalendarEvent ToModel(this CreateCalendarEventRequest request)
        {
            var response = new CalendarEvent
            {
                Title = request.Title,
                WorkoutPlanId = request.WorkoutPlanId,
                Start = request.Start,
                End = request.End,
                AllDay = request.AllDay
            };

            return response;
        }
    }
}
