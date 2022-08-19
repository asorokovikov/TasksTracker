namespace TasksTracker.Tests.Tasks;

public sealed class RemoveTaskTests : IClassFixture<TrackerWebApplicationFactory> {
    private readonly HttpClient _client;

    public RemoveTaskTests(TrackerWebApplicationFactory factory) => 
        _client = factory.CreateClient();

    [Fact]
    public async Task ValidRequest_ShouldReturn_200() {
        var task = await _client.GetFirstTaskItem();
        var response = await _client.DeleteAsync($"api/tasks/{task.TaskId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        response = await _client.GetAsync($"api/tasks/{task.TaskId}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task NotExistingId_ShouldReturn_404() {
        var response = await _client.DeleteAsync($"/api/tasks/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}