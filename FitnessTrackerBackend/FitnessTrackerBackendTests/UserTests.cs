using FitnessTrackerBackend.Dto.Mappings;
using FitnessTrackerBackend.Dto.Users;
using FitnessTrackerBackend.Models;
using FluentAssertions;

namespace FitnessTrackerBackendTests
{
    public class UserTests
    {
        [Fact]
        public void User_ToResponse_DoesNotExposePassword()
        {
            var user = new User
            {
                Id = new Guid(),
                Age = 12,
                Email = "",
                Gender = "Male",
                Height = 178,
                Name = "Test user",
                Weight = 12
            };

            var dto = user.ToResponse();

            dto.Name.Should().Be("Test user");
            dto.Age.Should().Be(12);

            typeof(UserResponse).GetProperties().Select(u => u.Name).Should().NotContain("Password");
        }
    }
}