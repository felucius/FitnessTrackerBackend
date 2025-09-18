using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.Users
{
    public sealed record CreateUserRequest
    (
        string? Gender,
        string Name,
        string Email,
        int? Age,
        int? Weight,
        int? Height,
        string? Password
    );
}
