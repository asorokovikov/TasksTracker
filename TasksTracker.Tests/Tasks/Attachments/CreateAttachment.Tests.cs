using TasksTracker.Tasks.Attachments.GettingAttachmentsList;

namespace TasksTracker.Tests.Tasks.Attachments;

public sealed class CreateAttachmentTests : IClassFixture<TrackerWebApplicationFactory> {
    private readonly ITestOutputHelper _output;
    private readonly HttpClient _client;

    public CreateAttachmentTests(ITestOutputHelper output, TrackerWebApplicationFactory factory) {
        _output = output;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ValidRequest_ShouldReturn_201() {
        const string fileContent = "Testing";
        var allTasks = await _client.GetFromJsonAsync<IReadOnlyList<TaskItem>>("/api/tasks");
        allTasks.Should().NotBeNull().And.NotBeEmpty();
        var taskId = allTasks![0].TaskId;
        await fileContent.AddStringToAttachmentAsync(taskId, _client);
    }

    [Fact]
    public async Task InvalidRequest_ShouldReturn_400() {
        using var formData = new MultipartFormDataContent();

        var allTasks = await _client.GetFromJsonAsync<IReadOnlyList<TaskItem>>("/api/tasks");
        allTasks.Should().NotBeNull().And.NotBeEmpty();
        var taskId = allTasks![0].TaskId;

        var response = await _client.PostAsync($"/api/tasks/{taskId}/attachments", formData);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Upload100MBFile_ShouldReturn_201() {
        const string filename = "largefile.bin";

        var allTasks = await _client.GetFromJsonAsync<IReadOnlyList<TaskItem>>("/api/tasks");
        allTasks.Should().NotBeNull().And.NotBeEmpty();
        var taskId = allTasks![0].TaskId;

        var payload = new byte[100 * 1024 * 1024];
        for (long index = 0; index < payload.LongLength; index++) {
            payload[index] = (byte) (index % byte.MaxValue);
        }

        using var stream = payload.ToMemoryStream();
        using var content = new StreamContent(stream);
        using var formData = new MultipartFormDataContent {
            { content, "file", filename }
        };

        var response = await _client.PostAsync($"/api/tasks/{taskId}/attachments", formData);
        var result = await response.Content.ReadAsStringAsync();
        _output.WriteLine(result);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var fileId = result.Unquoted();
        _output.WriteLine($"Uploaded file {fileId} ({payload.LongLength.FromBytes().Humanize()})");

        response = await _client.GetAsync($"/api/tasks/{taskId}/attachments");
        _output.WriteLine(await response.Content.ReadAsStringAsync());

        var attachments = await response.Content.ReadFromJsonAsync<IReadOnlyList<AttachmentItem>>();
        attachments.ShouldNotBeNull();
        var attachmentItem = attachments.FirstOrDefault(x => x.Id.ToString() == fileId);
        attachmentItem.ShouldNotBeNull().Should().BeEquivalentTo(
            new AttachmentItem(Guid.Parse(fileId), filename, payload.LongLength)
        );

        // GET File
        response = await _client.GetAsync($"/api/tasks/{taskId}/attachments/{fileId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var downloadedPayload = await response.Content.ReadAsByteArrayAsync();
        downloadedPayload.LongLength.Should().Be(payload.LongLength);
        downloadedPayload.Should().Equal(payload);
    }
}