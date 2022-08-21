using System.Diagnostics;
using TasksTracker.Api.Middleware;
using TasksTracker.Tasks.CreatingTask;

namespace TasksTracker.Tests.Tasks;

public sealed class CreateTaskTests : IClassFixture<TrackerWebApplicationFactory> {
    private readonly HttpClient _client;

    public CreateTaskTests(TrackerWebApplicationFactory factory) =>
        _client = factory.CreateClient();

    [Theory]
    [InlineData("task", "new")]
    [InlineData("task", "inprogress")]
    [InlineData("task", "Done")]
    public async Task ValidRequest_ShouldReturn_201(string? name, string? state) {
        var request = new CreateTaskRequest(Name: name, State: state);
        var response = await _client.PostAsync("/api/tasks", request.ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("task", null)]
    [InlineData(null, "New")]
    [InlineData("task", "random")]
    [InlineData("task", "new1")]
    [InlineData("", "new1")]
    public async Task InvalidRequest_ShouldReturn_400(string? name, string? state) {
        var request = new CreateTaskRequest(Name: name, State: state);
        var response = await _client.PostAsync("/api/tasks", request.ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseString = await response.Content.ReadAsStringAsync();
        var errorDetails = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        errorDetails.Should().NotBeNull();
        Debug.Assert(errorDetails != null);
        errorDetails.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
    }
}