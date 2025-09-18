using FitnessTrackerBackend.Dto.Users;
using FitnessTrackerBackend.Dto.WorkoutPlans;
using FitnessTrackerBackend.Models;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace FitnessTrackerBackendTests
{
    public class WorkoutPlansControllerTests : IClassFixture<CustomWebAppFactory>
    {
        private readonly HttpClient _client;

        public WorkoutPlansControllerTests(CustomWebAppFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnSeededWorkoutPlans()
        {
            var res = await _client.GetAsync("/api/workoutPlans");
            res.StatusCode.Should().Be(HttpStatusCode.OK);

            var workoutPlans = await res.Content.ReadFromJsonAsync<List<UserResponse>>();
            workoutPlans.Should().NotBeNull();
            workoutPlans!.Count.Should().Be(1);
        }

        [Fact]
        public async Task GetById_ReturnWorkoutPlans_WhenExists()
        {
            var id = "caaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
            var res = await _client.GetAsync($"/api/workoutPlans/{id}");
            res.StatusCode.Should().Be(HttpStatusCode.OK);

            var workoutPlan = await res.Content.ReadFromJsonAsync<WorkoutPlanResponse>();
            workoutPlan.Should().NotBeNull();
            workoutPlan!.Id.Should().Be(Guid.Parse(id));
        }

        [Fact]
        public async Task CreateWorkoutPlan_Return201_And_CanBeFetched()
        {
            var request = new CreateWorkoutPlanRequest(
                UserId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                User: null,
                Name: "Full body workout",
                Type: "Cardio",
                Description: "Training for endurance",
                Frequency: 2
            );

            var create = await _client.PostAsJsonAsync("/api/workoutPlans", request);
            create.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await create.Content.ReadFromJsonAsync<WorkoutPlanResponse>();
            created.Should().NotBeNull();
            created!.Name.Should().Be("Full body workout");

            // Follow the location
            var get = await _client.GetAsync($"/api/workoutPlans/{created.Id}");
            get.EnsureSuccessStatusCode();
            var fetched = await get.Content.ReadFromJsonAsync<WorkoutPlanResponse>();
            fetched!.Name.Should().Be("Full body workout");
        }
    }
}
