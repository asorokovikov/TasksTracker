namespace TasksTracker.Tests.Tasks;

public sealed class 
GetTasksListTests : IClassFixture<TrackerWebApplicationFactory> {
    private readonly HttpClient _client;

    public GetTasksListTests(TrackerWebApplicationFactory factory) => 
        _client = factory.CreateClient();

    [Fact]
    public async Task ValidRequest_ShouldReturn_200() {
        var response = await _client.GetAsync($"/api/tasks");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tasks = await response.Content.ReadFromJsonAsync<IReadOnlyList<TaskItem>>();
        tasks.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task ValidRequest_With_Paging_ShouldReturn_200() {
        const int page = 2;
        const int pageSize = 1;
        
        var allTasks = await _client.GetFromJsonAsync<IReadOnlyList<TaskItem>>($"/api/tasks");
        allTasks.Should().NotBeNull().And.NotBeEmpty();
        
        var response = await _client.GetAsync($"/api/tasks/?page={page}&pageSize={pageSize}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<TaskItem>>();
        result.Should().BeEquivalentTo(new List<TaskItem> { allTasks![1] });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999)]
    public async Task NegativeOrZeroPage_ShouldReturn_400(int page) {
        var response = await _client.GetAsync($"/api/tasks/?page={page}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999)]
    public async Task NegativeOrZeroPageSize_ShouldReturn_400(int pageSize) {
        var response = await _client.GetAsync($"/api/tasks/?pageSize={pageSize}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}