using TasksTracker.Tasks.Attachments.GettingAttachmentsList;

namespace TasksTracker.Tests.Tasks.Attachments;

public sealed class 
GetAttachmentListTests : IClassFixture<TrackerWebApplicationFactory> {
    private readonly ITestOutputHelper _output;
    private readonly HttpClient _client;
    
    public GetAttachmentListTests(ITestOutputHelper output, TrackerWebApplicationFactory factory) {
        _output = output;
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task ValidRequest_ShouldReturn_200() {
        const string fileContent = "Testing";
        const string filename = "hello.txt";

        var task = await _client.GetFirstTaskItem();
        var taskId = task.TaskId;
        
        var fileId = await fileContent.AddStringToAttachmentAsync(taskId, _client, filename);
        _output.WriteLine(fileId);        
        
        var response = await _client.GetAsync($"/api/tasks/{taskId}/attachments");
        var items = await response.Content.ReadFromJsonAsync<IReadOnlyList<AttachmentItem>>();
        items.Should().NotBeNull().And.NotBeEmpty();
        var item = items![0];
        item.Id.Should().Be(fileId);
        item.Filename.Should().Be(filename);
    }
}
