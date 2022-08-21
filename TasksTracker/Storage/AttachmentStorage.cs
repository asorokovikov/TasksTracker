using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TasksTracker.Common;

namespace TasksTracker.Storage;

internal sealed class AttachmentConfiguration {
    public string ContentRootDirectory { get; set; } = default!;
    public long FileSizeLimit { get; set; }
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
    private readonly AttachmentConfiguration Configuration;
    
    private string ContentRootDirectory => Configuration.ContentRootDirectory;

    public LocalAttachmentClientFactory(
        IOptions<AttachmentConfiguration> configuration,
        ILoggerFactory loggerFactory
    ) {
        _loggerFactory = loggerFactory;
        Configuration = configuration.Value;
    }

    public IAttachmentClient CreateClient(Guid taskId) {
        var directory = ContentRootDirectory.AppendPath(taskId.ToString());
        var logger = _loggerFactory.CreateLogger<LocalAttachmentClient>();
        return new LocalAttachmentClient(directory, Configuration.FileSizeLimit, logger);
    }
}

public sealed class LocalAttachmentClient : IAttachmentClient {
    private readonly string _rootDirectory;
    private readonly long _fileSizeLimit;
    private readonly ILogger _logger;

    public LocalAttachmentClient(
        string rootDirectory, 
        long fileSizeLimit, 
        ILogger<LocalAttachmentClient> logger
    ) {
        _rootDirectory = rootDirectory.CreateDirectoryIfNotExists();
        _fileSizeLimit = fileSizeLimit;
        _logger = logger;
    }

    public async Task UploadAsync(Guid fileId, IFormFile formFile, CancellationToken ct = default) {
        if (formFile.Length > _fileSizeLimit)
            throw new ArgumentException($"The file is too large. " +
                $"Expecting length of the file to be less than {_fileSizeLimit.FromBytes().Humanize()}");
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