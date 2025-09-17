using FitnessTrackerBackend.Dto.Users;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace FitnessTrackerBackendTests
{
    public class UsersControllerTests : IClassFixture<CustomWebAppFactory>
    {
        private readonly HttpClient _client;

        public UsersControllerTests(CustomWebAppFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsSeededUsers()
        {
            var res = await _client.GetAsync("/api/users");
            res.StatusCode.Should().Be(HttpStatusCode.OK);

            var users = await res.Content.ReadFromJsonAsync<List<UserResponse>>();
            users.Should().NotBeNull();
            users!.Select(u => u.Email).Should().BeSubsetOf(new[] { "alice@example.com", "bob@example.com" });
        }

        [Fact]
        public async Task GetById_ReturnsUser_WhenExists()
        {
            var id = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
            var res = await _client.GetAsync($"/api/users/{id}");
            res.StatusCode.Should().Be(HttpStatusCode.OK);

            var user = await res.Content.ReadFromJsonAsync<UserResponse>();
            user.Should().NotBeNull();
            user!.Id.Should().Be(Guid.Parse(id));
            user.Email.Should().Be("alice@example.com");
        }

        [Fact]
        public async Task GetById_Returns404_WhenMissing()
        {
            var res = await _client.GetAsync($"/api/users/{Guid.NewGuid()}");
            res.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_Returns201_And_CanBeFetched()
        {
            var req = new CreateUserRequest(
                Gender: "M",
                Name: "Carl",
                Email: "carl@example.com",
                Age: null,
                Weight: null,
                Height: null,
                Password: "pass"
            );

            var create = await _client.PostAsJsonAsync("/api/users", req);
            create.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await create.Content.ReadFromJsonAsync<UserResponse>();
            created.Should().NotBeNull();
            created!.Email.Should().Be("carl@example.com");

            // follow the location
            var get = await _client.GetAsync($"/api/users/{created.Id}");
            get.EnsureSuccessStatusCode();
            var fetched = await get.Content.ReadFromJsonAsync<UserResponse>();
            fetched!.Email.Should().Be("carl@example.com");
        }

        [Fact]
        public async Task GetByEmail_ReturnsUser_WhenExists()
        {
            var res = await _client.GetAsync("/api/users/by-email?email=alice@example.com");
            res.StatusCode.Should().Be(HttpStatusCode.OK);

            var user = await res.Content.ReadFromJsonAsync<UserResponse>();
            user!.Email.Should().Be("alice@example.com");
        }

        [Fact]
        public async Task GetByEmail_Returns404_WhenMissing()
        {
            var res = await _client.GetAsync("/api/users/by-email?email=not@found.test");
            res.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
