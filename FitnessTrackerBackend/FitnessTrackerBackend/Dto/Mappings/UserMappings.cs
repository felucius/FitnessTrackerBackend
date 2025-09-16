using FitnessTrackerBackend.Dto.Users;
using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.Mappings
{
    public static class UserMappings
    {
        public static UserResponse ToResponse(this User user)
        {
            var response = new UserResponse(
                Id: user.Id,
                Gender: user.Gender,
                Name: user.Name,
                Email: user.Email,
                Age: user.Age,
                Weight: user.Weight,
                Height: user.Height
            );

            return response;
        }

        public static User ToModel(this CreateUserRequest request)
        {
            var response = new User
            {
                Gender = request.Gender,
                Name = request.Name,
                Email = request.Email,
                Age = request.Age,
                Weight = request.Weight,
                Height = request.Height,
                Password = request.Password
            };

            return response;
        }
    }
}
