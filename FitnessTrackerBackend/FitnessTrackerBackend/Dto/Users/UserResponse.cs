namespace FitnessTrackerBackend.Dto.Users
{
    public sealed record UserResponse
    (
        Guid Id,
        string? Gender,
        string Name,
        string Email,
        int? Age,
        int? Weight,
        int? Height
    );
}
