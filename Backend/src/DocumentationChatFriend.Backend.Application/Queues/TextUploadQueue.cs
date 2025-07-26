using System.Threading.Channels;
using DocumentationChatFriend.Backend.Domain.Models;

namespace DocumentationChatFriend.Backend.Application.Queues;

public class TextUploadQueue
{
    private readonly Channel<TextUploadJob> _channel;

    public TextUploadQueue()
    {
        var options = new BoundedChannelOptions(1000)
        {
            FullMode = BoundedChannelFullMode.Wait
        };

        _channel = Channel.CreateBounded<TextUploadJob>(options);
    }

    public async ValueTask EnqueueAsync(TextUploadJob job, CancellationToken cancellationToken = default)
    {
        await _channel.Writer.WriteAsync(job, cancellationToken);
    }

    public IAsyncEnumerable<TextUploadJob> DequeueAsync(
        CancellationToken cancellationToken = default)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}