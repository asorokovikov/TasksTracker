using System.Net.Http.Json;
using System.Text;
using TasksTracker.Common;
using TasksTracker.Tasks;

namespace TasksTracker.Tests;

public static class TestHelper {
    public static StringContent ToStringContent<T>(this T value) where T : notnull =>
        new(value.ToJson(), Encoding.UTF8, "application/json");
    
    public static async Task<string>
    AddStringToAttachmentAsync(this string @string, Guid taskId, HttpClient client, string filename = "file.txt") {
        using var stream = @string.Utf8ToBytes().ToMemoryStream();
        using var content = new StreamContent(stream);
        using var formData = new MultipartFormDataContent();
        formData.Add(content, "file", filename);
        
        var response = await client.PostAsync($"/api/tasks/{taskId}/attachments", formData);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadAsStringAsync();
        return result.Unquoted();
    }

    public static async Task<TaskItem>
    GetFirstTaskItem(this HttpClient client) {
        var tasks = await client.GetFromJsonAsync<IReadOnlyList<TaskItem>>($"/api/tasks/?pageSize=1");
        tasks.Should().NotBeNull().And.NotBeEmpty();
        return tasks![0];
    }
}