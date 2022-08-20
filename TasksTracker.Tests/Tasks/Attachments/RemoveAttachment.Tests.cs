using TasksTracker.Tasks.Attachments.GettingAttachmentsList;

namespace TasksTracker.Tests.Tasks.Attachments;

public sealed class RemoveAttachmentTests : IClassFixture<TrackerWebApplicationFactory> {
    private readonly ITestOutputHelper _output;
    private readonly HttpClient _client;

    public RemoveAttachmentTests(ITestOutputHelper output, TrackerWebApplicationFactory factory) {
        _output = output;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ValidRequest_ShouldReturn_200() {
        const string fileContent = "Testing";

        var task = await _client.GetFirstTaskItem();
        var taskId = task.TaskId;

        var fileId = await fileContent.AddStringToAttachmentAsync(taskId, _client);
        _output.WriteLine(fileId);

        var response = await _client.DeleteAsync($"/api/tasks/{taskId}/attachments/{fileId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response = await _client.GetAsync($"/api/tasks/{taskId}/attachments");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<IReadOnlyList<AttachmentItem>>();
        items.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public async Task InvalidRequest_ShouldReturn_400() {
        var task = await _client.GetFirstTaskItem();
        var taskId = task.TaskId;

        var response = await _client.DeleteAsync($"/api/tasks/{taskId}/attachments/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

}
