using TasksTracker.Tasks.CreatingTask;

namespace TasksTracker.Tests.Tasks;

public sealed class 
GetTaskTests : IClassFixture<TrackerWebApplicationFactory> {
    private readonly HttpClient _client;

    public GetTaskTests(TrackerWebApplicationFactory factory) => 
        _client = factory.CreateClient();

    [Fact]
    public async Task ValidRequest_ShouldReturn_200() {
        var task = await _client.GetFirstTaskItem();
        var validId = task.TaskId;
        var response = await _client.GetAsync($"/api/tasks/{validId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("hello")]
    [InlineData("123")]
    public async Task InvalidGuidId_ShouldReturn_404(string guid) {
        var response = await _client.GetAsync($"/api/tasks/{guid}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task NotExistingId_ShouldReturn_404() {
        var response = await _client.GetAsync($"/api/tasks/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateTask_GetTask_Test() {
        var request = new CreateTaskRequest(Name: "Task #1", State: "New");
        var response = await _client.PostAsync("/api/tasks", request.ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        response = await _client.GetAsync(response.Headers.Location);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<TaskItem>();
        result.Should().NotBeNull();
        result!.Name.Should().Be(request.Name);
        result!.State.Should().Be(request.State);
    }

}