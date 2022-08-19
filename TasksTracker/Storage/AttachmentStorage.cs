using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TasksTracker.Common;

namespace TasksTracker.Storage;

internal sealed class AttachmentConfiguration {
    public string ContentRootDirectory { get; set; } = "Attachments";
}

public interface IAttachmentClient {
    Task UploadAsync(Guid fileId, IFormFile formFile, CancellationToken ct = default);
    Task DownloadToAsync(Stream target, Guid fileId, CancellationToken ct = default);
    Task DeleteAsync(Guid fileId, CancellationToken ct = default);
}

public interface IAttachmentClientFactory {
    public IAttachmentClient CreateClient(Guid taskId);
}

internal sealed class LocalAttachmentClientFactory : IAttachmentClientFactory {
    private readonly ILoggerFactory _loggerFactory;
    private readonly string _contentRootDirectory;

    public LocalAttachmentClientFactory(
        IOptions<AttachmentConfiguration> configuration,
        ILoggerFactory loggerFactory
    ) {
        _loggerFactory = loggerFactory;
        _contentRootDirectory = configuration.Value.ContentRootDirectory;
    }

    public IAttachmentClient CreateClient(Guid taskId) {
        var directory = _contentRootDirectory.AppendPath(taskId.ToString());
        var logger = _loggerFactory.CreateLogger<LocalAttachmentClient>();
        return new LocalAttachmentClient(directory, logger);
    }
}

public sealed class LocalAttachmentClient : IAttachmentClient {
    private readonly string _rootDirectory;
    private readonly ILogger _logger;

    public LocalAttachmentClient(string rootDirectory, ILogger<LocalAttachmentClient> logger) {
        _rootDirectory = rootDirectory.CreateDirectoryIfNotExists();
        _logger = logger;
    }

    public async Task UploadAsync(Guid fileId, IFormFile formFile, CancellationToken ct = default) {
        var file = GetFile(fileId);
        await using var stream = file.OpenFileForWrite();
        await formFile.CopyToAsync(stream, ct);
        _logger.LogInformation("Saved attachment {Filename} to {Path}", formFile.FileName, file);
    }

    public async Task DownloadToAsync(Stream target, Guid fileId, CancellationToken ct = default) {
        var file = GetFile(fileId);
        await using var stream = file.OpenFileForRead();
        await stream.CopyToAsync(target, ct);
        _logger.LogInformation("Downloaded attachment {FileID}", fileId);
    }

    public Task DeleteAsync(Guid fileId, CancellationToken ct = default) {
        var file = GetFile(fileId);
        try {
            file.DeleteFileIfExists();
            _logger.LogInformation("Attachment deleted {FileId}", fileId);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to delete attachment with id={FileId}", fileId.ToString().Quoted());
        }
        return Task.CompletedTask;
    }

    private string GetFile(Guid fileId) => _rootDirectory.AppendPath(fileId.ToString());
}