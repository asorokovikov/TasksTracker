namespace TasksTracker.Tests.Tasks.Attachments; 

public sealed class 
CreateAttachmentTests : IClassFixture<TrackerWebApplicationFactory> {
    private readonly ITestOutputHelper _output;
    private readonly TrackerWebApplicationFactory _factory;
    private readonly HttpClient _client;
    
    private string ContentRootDirectory => _factory.ContentRootDirectory;

    public CreateAttachmentTests(ITestOutputHelper output, TrackerWebApplicationFactory factory) {
        _output = output;
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ValidRequest_ShouldReturn_201() {
        const string fileContent = "Testing";
        var allTasks = await _client.GetFromJsonAsync<IReadOnlyList<TaskItem>>($"/api/tasks");
        allTasks.Should().NotBeNull().And.NotBeEmpty();
        var taskId = allTasks![0].TaskId;
        await fileContent.AddStringToAttachmentAsync(taskId, _client);
    }
    
    [Fact]
    public async Task InvalidRequest_ShouldReturn_400() {
        using var formData = new MultipartFormDataContent();
        
        var allTasks = await _client.GetFromJsonAsync<IReadOnlyList<TaskItem>>($"/api/tasks");
        allTasks.Should().NotBeNull().And.NotBeEmpty();
        var taskId = allTasks![0].TaskId;
        
        var response = await _client.PostAsync($"/api/tasks/{taskId}/attachments", formData);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}