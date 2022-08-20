namespace TasksTracker.Tests.Tasks.Attachments;

public sealed class GetAttachmentTests : IClassFixture<TrackerWebApplicationFactory> {
    private readonly ITestOutputHelper _output;
    private readonly HttpClient _client;

    public GetAttachmentTests(ITestOutputHelper output, TrackerWebApplicationFactory factory) {
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

        var allAttachments = await _client.GetAsync($"/api/tasks/{taskId}/attachments");
        _output.WriteLine(await allAttachments.Content.ReadAsStringAsync());
        var response = await _client.GetAsync($"/api/tasks/{taskId}/attachments/{fileId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadAsByteArrayAsync();
        var result = payload.ToUtf8String();
        result.Should().Be(fileContent);
        _output.WriteLine(result);
    }
}
