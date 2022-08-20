using TasksTracker.Tasks.UpdatingTask;

namespace TasksTracker.Tests.Tasks;

public sealed class UpdateTaskTests : IClassFixture<TrackerWebApplicationFactory> {
    private readonly HttpClient _client;

    public UpdateTaskTests(TrackerWebApplicationFactory factory) =>
        _client = factory.CreateClient();

    [Fact]
    public async Task ValidRequest_ShouldReturn_200() {
        var (taskId, _, state, createdAt) = await _client.GetFirstTaskItem();
        var request = new UpdateTaskRequest(taskId, "123", state, createdAt);
        var response = await _client.PutAsync($"/api/tasks/{taskId}", request.ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response = await _client.GetAsync($"/api/tasks/{taskId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<TaskItem>();
        var taskItem = result.VerifyNotNull();
        taskItem.TaskId.Should().Be(taskId);
        taskItem.Name.Should().Be("123");
        taskItem.State.Should().Be(state);
        taskItem.CreatedAt.Should().Be(createdAt);
    }

    [Fact]
    public async Task InvalidRequest_ShouldReturn_400() {
        var request = new UpdateTaskRequest(null, null, null, null);
        var response = await _client.PutAsync($"/api/tasks/{Guid.NewGuid()}", request.ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}