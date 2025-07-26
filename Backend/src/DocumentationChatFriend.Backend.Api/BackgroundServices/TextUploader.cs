using DocumentationChatFriend.Backend.Application.Queues;
using DocumentationChatFriend.Backend.Domain.Interfaces;

namespace DocumentationChatFriend.Backend.Api.BackgroundServices;

public class TextUploader : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TextUploadQueue _queue;
    private readonly ILogger<TextUploader> _logger;
    public TextUploader(TextUploadQueue queue, ILogger<TextUploader> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _queue = queue;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        await foreach (var job in _queue.DequeueAsync(ct))
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var textUploadService = scope.ServiceProvider.GetRequiredService<ITextUploadService>();
            try
            {
                await textUploadService.UpsertAsync(job);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to process text upload: {collectionName}, {text}",
                    job.CollectionName,
                    Truncate(job.Text, 50) + "[...]"
                );


            }
        }
    }

    private string Truncate(string text, int maxChars)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        return text.Length > maxChars ? text.Substring(0, maxChars) : text;
    }
}